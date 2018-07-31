namespace DotNetStarter
{
    using Abstractions;
    using Abstractions.Internal;
    using DotNetStarter.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default startup handler
    /// </summary>
    public class StartupHandler : IStartupHandler, IStartupEngine
    {
        private readonly bool _enableDelayedStartupModules = false;
        private readonly ILocatorDefaultRegistrations _locatorDefaultRegistrations;
        private readonly ILocatorRegistry _locatorRegistry;
        private readonly Func<ITimedTask> _timedTaskFactory;
        private Action _delayedStartupModules;
        private bool _locatorStartupInvoked = false;

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
        /// Fires after ILocatorConfigure.Configure has completed in all executing modules
        /// </summary>
        public event Action OnLocatorStartupComplete
        {
            add
            {
                if (_locatorStartupInvoked == true)
                {
                    throw new Exception($"Locator startup complete has already been invoked, try {nameof(OnStartupComplete)} instead!");
                }

                _OnLocatorStartupComplete += value;
            }
            remove
            {
                _OnLocatorStartupComplete -= value;
            }
        }

        /// <summary>
        /// Fires after IStartupModule.Startup has completed in all executing modules
        /// </summary>
        public event Action OnStartupComplete;

        private event Action _OnLocatorStartupComplete;

        /// <summary>
        /// Startup Configuration for IStartupEngine
        /// </summary>
        public IStartupConfiguration Configuration { get; protected set; }

        /// <summary>
        /// ILocator for IStartupEngine
        /// </summary>
        public ILocator Locator { get; protected set; }

        /// <summary>
        /// Starup process, by default it scans assemblies, sorts modules, configures container, and runs startup for each module
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual IStartupContext ConfigureLocator(IStartupConfiguration config)
        {
            Configuration = config;
            IStartupEngine startupEngine = this;
            IEnumerable<Assembly> assemblies = config.Assemblies;
            IStartupContext startupContext = null;
            IEnumerable<IDependencyNode> sortedModules = null;
            IEnumerable<IDependencyNode> filteredModules = null;
            var timerNameBase = typeof(StartupHandler).FullName;

            // scan the assemblies for registered types for quick retrieval
            var scanSetup = _timedTaskFactory.Invoke();
            scanSetup.Name = timerNameBase + ".AssemblyScan";
            scanSetup.TimedAction = () =>
            {
                var discoverTypeAttrs = assemblies.SelectMany(x => x.CustomAttribute(typeof(DiscoverTypesAttribute), false).OfType<DiscoverTypesAttribute>());
                var discoverTypes = discoverTypeAttrs.SelectMany(x => x.DiscoverTypes);
                Func<Assembly, bool> assemblyFilter = null; // a custom config may set this to null

                if (config.AssemblyFilter != null)
                {
                    assemblyFilter = config.AssemblyFilter.FilterAssembly;
                }

                config.AssemblyScanner.Scan(assemblies, discoverTypes, assemblyFilter);
            };

            // modules with attribute
            var moduleSortSetup = _timedTaskFactory.Invoke();
            moduleSortSetup.Name = timerNameBase + ".ModuleSort";
            moduleSortSetup.TimedAction = () =>
            {
                var dependents = config.AssemblyScanner.GetTypesFor(typeof(StartupModuleAttribute)).OfType<object>();
                sortedModules = config.DependencySorter.Sort<StartupModuleAttribute>(dependents);
                var tempFiltered = config.ModuleFilter?.FilterModules(sortedModules) ?? sortedModules;

                // ensure module order wasn't tampered with
                filteredModules = from i in sortedModules join o in tempFiltered on i.FullName equals o.FullName select o;
            };

            // create container
            var containerSetup = _timedTaskFactory.Invoke(); ;
            containerSetup.Name = timerNameBase + ".ContainerSetup";
            containerSetup.TimedAction = () =>
            {
                if (_locatorRegistry == null) { throw new NullLocatorException(); }
                _locatorDefaultRegistrations.Configure(_locatorRegistry, filteredModules, config);
                var locatorRegistries = (_locatorRegistry as ILocatorResolveConfigureModules)?.ResolveConfigureModules(filteredModules, config) ?? (_locatorRegistry as ILocator).GetAll<ILocatorConfigure>();

                ConfigureRegistry(_locatorRegistry, locatorRegistries);

                var readOnlyLocator = ReadOnlyLocator.CreateReadOnlyLocator(_locatorRegistry as ILocator);
                _locatorRegistry.Add(typeof(ILocator), readOnlyLocator);
                startupContext = CreateStartupContext(readOnlyLocator, filteredModules, sortedModules, config);
                _locatorRegistry.Add(typeof(IStartupContext), startupContext);

                ImportHelper.OnEnsureLocator += (() => readOnlyLocator); // configure import<T> locator
                _OnLocatorStartupComplete?.Invoke(); //execute locator complete before verification since last minute additions can occur here.
                _locatorStartupInvoked = true;
                (_locatorRegistry as ILocatorVerification)?.Verify();
            };

            // startup modules
            var startupModulesTask = _timedTaskFactory.Invoke();
            startupModulesTask.Name = timerNameBase + ".StartupModules";
            startupModulesTask.TimedAction = () =>
            {
                Locator = _locatorRegistry as ILocator;// objectFactory.CreateRegistry(config) as ILocator;
                if (!(Locator is ILocatorRegistry)) { throw new NullLocatorException(); }

                var modules = Locator.GetAll<IStartupModule>(); // resolve all startup modules for DI
                ExecuteStartupModules(modules);
                OnStartupComplete?.Invoke();
            };

            // execute tasks in order
            config.TimedTaskManager.Execute(scanSetup);
            config.TimedTaskManager.Execute(moduleSortSetup);
            config.TimedTaskManager.Execute(containerSetup);

            // optionally allows delaying startup until later, must be implemented on IStartupConfiguration instances
            void startup() => config.TimedTaskManager.Execute(startupModulesTask);

            if (_enableDelayedStartupModules)
            {
                _delayedStartupModules = startup;
            }
            else
            {
                startup();
            }

            return startupContext;
        }

        /// <summary>
        /// Tries to run IStartupModules if delayed execute is enabled
        /// </summary>
        public bool TryExecuteStartupModules()
        {
            if(_delayedStartupModules == null) { return false; }
            _delayedStartupModules.Invoke();
            _delayedStartupModules = null;
            return true;
        }

        /// <summary>
        /// Configures ILocatorConfigure modules
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="locatorRegistries"></param>
        protected virtual void ConfigureRegistry(ILocatorRegistry registry, IEnumerable<ILocatorConfigure> locatorRegistries)
        {
            foreach (var map in locatorRegistries ?? Enumerable.Empty<ILocatorConfigure>())
            {
                map.Configure(registry, this);
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
        protected virtual void ExecuteStartupModules(IEnumerable<IStartupModule> modules)
        {
            foreach (var x in modules ?? Enumerable.Empty<IStartupModule>())
            {
                x.Startup(this);
            }
        }
    }
}