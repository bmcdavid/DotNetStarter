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
        private bool _ranConfigure = false;

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
            Locator = _locatorRegistry as ILocator;// objectFactory.CreateRegistry(config) as ILocator;
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

                // a custom config may set this to null
                Func<Assembly, bool> assemblyFilter = null;

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
                if (!(Locator is ILocatorRegistry registry))
                    throw new NullLocatorException();

                _locatorDefaultRegistrations.Configure(registry, filteredModules, config);
                var locatorRegistries = (registry as ILocatorResolveConfigureModules)?.ResolveConfigureModules(filteredModules, config)
                                            ?? (registry as ILocator).GetAll<ILocatorConfigure>();

                foreach (var map in locatorRegistries ?? Enumerable.Empty<ILocatorConfigure>())
                {
                    map.Configure(registry, this);
                }

                var readOnlyLocator = Internal.ReadOnlyLocator.CreateReadOnlyLocator(registry as ILocator);
                registry.Add(typeof(ILocator), readOnlyLocator);

                startupContext = CreateStartupContext(readOnlyLocator, filteredModules, sortedModules, config);
                registry.Add(typeof(IStartupContext), startupContext);

                ImportHelper.OnEnsureLocator += (() => readOnlyLocator); // configure import<T> locator

                _OnLocatorStartupComplete?.Invoke(); //execute locator complete before verification since last minute additions can occur here.
                _locatorStartupInvoked = true;
                (registry as ILocatorVerification)?.Verify();
            };

            // startup modules
            var startupModulesTask = _timedTaskFactory.Invoke();
            startupModulesTask.Name = timerNameBase + ".StartupModules";
            startupModulesTask.TimedAction = () =>
            {
                var modules = Locator.GetAll<IStartupModule>(); // resolve all startup modules for DI
                Startup(modules);
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
        /// MUST execute after Startup, used to run IStartupModules when enableDelayedStartup is used
        /// </summary>
        public void StartupModules()
        {
            //todo: how to fix delayed start, seems odd to split in a dependent call

            _delayedStartupModules.Invoke();
            _delayedStartupModules = null;
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
        /// Startups up given modules
        /// </summary>
        /// <param name="modules"></param>
        protected virtual void Startup(IEnumerable<IStartupModule> modules)
        {
            var startupModules = modules ?? Enumerable.Empty<IStartupModule>();

            foreach (var x in startupModules)
            {
                x.Startup(this);
            }
        }
    }
}