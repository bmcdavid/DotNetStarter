using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using DotNetStarter.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter
{
    /// <summary>
    /// Default startup handler
    /// </summary>
    public class StartupHandler : IStartupHandler
    {
        private static readonly string _timerNameBase = typeof(StartupHandler).FullName;
        private readonly bool _enableDelayedStartupModules = false;
        private readonly ILocatorDefaultRegistrations _locatorDefaultRegistrations;
        private readonly ILocatorRegistry _locatorRegistry;
        private readonly Func<ITimedTask> _timedTaskFactory;
        private Action _delayedStart;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        /// <param name="locatorRegistry"></param>
        /// <param name="locatorDefaultRegistrations"></param>
        /// <param name="enableDelayedStartupModules">If true, doesn't run IStartupModules until IStartupHandler.StartupModules is invoked, default is true</param>
        public StartupHandler(Func<ITimedTask> timedTaskFactory, ILocatorRegistry locatorRegistry, ILocatorDefaultRegistrations locatorDefaultRegistrations, bool enableDelayedStartupModules = true)
        {
            _timedTaskFactory = timedTaskFactory;
            _locatorRegistry = locatorRegistry;
            _locatorDefaultRegistrations = locatorDefaultRegistrations;
            _enableDelayedStartupModules = enableDelayedStartupModules;
        }

        /// <summary>
        /// Starup process, by default it scans assemblies, sorts modules, configures container, and runs startup for each module
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual IStartupContext ConfigureLocator(IStartupConfiguration config)
        {
            var configurationArgs = new StartupEngineConfigurationArgs(config);
            IStartupContext startupContext = null;
            ReadOnlyLocator readOnlyLocator = null;
            ICollection<IDependencyNode> sortedModules = null;
            ICollection<IDependencyNode> filteredModules = null;

            // scan the assemblies for registered types for quick retrieval
            var scanSetup = _timedTaskFactory.Invoke();
            scanSetup.Name = _timerNameBase + ".AssemblyScan";
            scanSetup.TimedAction = () =>
            {
                var assemblies = config.Assemblies;
                var discoverTypeAttrs = assemblies.SelectMany(x => x.CustomAttribute(typeof(DiscoverTypesAttribute), false).OfType<DiscoverTypesAttribute>());
                var discoverTypes = discoverTypeAttrs.SelectMany(x => x.DiscoverTypes);
                Func<System.Reflection.Assembly, bool> assemblyFilter = null; // a custom config may set this to null

                if (config.AssemblyFilter != null)
                {
                    assemblyFilter = config.AssemblyFilter.FilterAssembly;
                }

                config.AssemblyScanner.Scan(assemblies, discoverTypes, assemblyFilter);
            };

            // modules with attribute
            var moduleSortSetup = _timedTaskFactory.Invoke();
            moduleSortSetup.Name = _timerNameBase + ".ModuleSort";
            moduleSortSetup.TimedAction = () =>
            {
                var dependents = config.AssemblyScanner.GetTypesFor(typeof(StartupModuleAttribute)).OfType<object>();
                sortedModules = config.DependencySorter.Sort<StartupModuleAttribute>(dependents);
                var tempFiltered = config.ModuleFilter?.FilterModules(sortedModules) ?? sortedModules;

                // ensure module order wasn't tampered with
                filteredModules = (from i in sortedModules join o in tempFiltered on i.FullName equals o.FullName select o).ToList();
            };

            // create container
            var containerSetup = _timedTaskFactory.Invoke(); ;
            containerSetup.Name = _timerNameBase + ".ContainerSetup";
            containerSetup.TimedAction = () =>
            {
                if (_locatorRegistry == null) { throw new NullLocatorException(); }
                _locatorDefaultRegistrations.Configure(_locatorRegistry, filteredModules, config);
                var locatorRegistries = (_locatorRegistry as ILocatorResolveConfigureModules)?.ResolveConfigureModules(filteredModules, config) ?? (_locatorRegistry as ILocator).GetAll<ILocatorConfigure>();

                ConfigureRegistry(_locatorRegistry, locatorRegistries, configurationArgs);

                readOnlyLocator = ReadOnlyLocator.CreateReadOnlyLocator(_locatorRegistry as ILocator);
                _locatorRegistry.Add(typeof(ILocator), readOnlyLocator);
                startupContext = CreateStartupContext(readOnlyLocator, filteredModules, sortedModules, config);
                _locatorRegistry.Add(typeof(IStartupContext), startupContext);

                ImportHelper.OnEnsureLocator += (() => readOnlyLocator); // configure import<T> locator
                configurationArgs.RaiseLocatorSetupComplete();
                (_locatorRegistry as ILocatorVerification)?.Verify();
            };

            // startup modules
            var startupModulesTask = _timedTaskFactory.Invoke();
            startupModulesTask.Name = _timerNameBase + ".StartupModules";
            startupModulesTask.TimedAction = () =>
            {
                var startupEngine = new StartupEngine(readOnlyLocator, configurationArgs);
                var modules = startupEngine.Locator.GetAll<IStartupModule>(); // resolve all startup modules for DI
                ExecuteStartupModules(modules, startupEngine);
                startupEngine.RaiseStartupComplete();
            };

            // execute tasks in order
            config.TimedTaskManager.Execute(scanSetup);
            config.TimedTaskManager.Execute(moduleSortSetup);
            config.TimedTaskManager.Execute(containerSetup);

            // optionally allows delaying startup until later, must be implemented on IStartupConfiguration instances
            _delayedStart = () => config.TimedTaskManager.Execute(startupModulesTask);

            if (!_enableDelayedStartupModules)
            {
                TryExecuteStartupModules();
            }

            return startupContext;
        }

        /// <summary>
        /// Tries to run IStartupModules if delayed execute is enabled
        /// </summary>
        public bool TryExecuteStartupModules()
        {
            if (_delayedStart == null) { return false; }
            _delayedStart.Invoke();
            _delayedStart = null;
            return true;
        }

        /// <summary>
        /// Configures ILocatorConfigure modules
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="locatorRegistries"></param>
        /// <param name="startupConfigurationEngine"></param>
        protected virtual void ConfigureRegistry(ILocatorRegistry registry, IEnumerable<ILocatorConfigure> locatorRegistries, IStartupEngineConfigurationArgs startupConfigurationEngine)
        {
            foreach (var map in locatorRegistries ?? Enumerable.Empty<ILocatorConfigure>())
            {
                map.Configure(registry, startupConfigurationEngine);
            }
        }

        /// <summary>
        /// Creates default startup context
        /// </summary>
        /// <param name="readOnlyLocator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="sortedModules"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual IStartupContext CreateStartupContext(ReadOnlyLocator readOnlyLocator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> sortedModules, IStartupConfiguration config)
        {
            return new StartupContext(readOnlyLocator, sortedModules, filteredModules, config);
        }

        /// <summary>
        /// Startups up given IStartupModule instances
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="startupEngine"></param>
        protected virtual void ExecuteStartupModules(IEnumerable<IStartupModule> modules, IStartupEngine startupEngine)
        {
            foreach (var x in modules ?? Enumerable.Empty<IStartupModule>())
            {
                x.Startup(startupEngine);
            }
        }
    }
}