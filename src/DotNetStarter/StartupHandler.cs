namespace DotNetStarter
{
    using Abstractions;
    using Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default startup handler
    /// </summary>
    public class StartupHandler : IStartupHandler, IStartupEngine
    {
        private bool _LocatorStartupInvoked = false;

        private bool _Started = false;

        private IStartupContext _Context;

        /// <summary>
        /// Fires after ILocatorConfigure.Configure has completed in all executing modules
        /// </summary>
        public event Action OnLocatorStartupComplete
        {
            add
            {
                if (_LocatorStartupInvoked == true)
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

#pragma warning disable CS0612 // Type or member is obsolete
                              /// <summary>
                              /// Starup process, by default it scans assemblies, sorts modules, configures container, and runs startup for each module
                              /// </summary>
                              /// <param name="config"></param>
                              /// <param name="objectFactory"></param>
                              /// <param name="context"></param>
                              /// <returns></returns>
        public virtual bool Startup(IStartupConfiguration config, IStartupObjectFactory objectFactory, out IStartupContext context)
#pragma warning restore CS0612 // Type or member is obsolete
        {
            if (_Started)
            {
                context = _Context;

                return false;
            }

            Configuration = config;
            Locator = objectFactory.CreateRegistry(config) as ILocator;

            IStartupEngine startupEngine = this;
            IEnumerable<Assembly> assemblies = config.Assemblies;
            IStartupContext tempContext = null;
            IEnumerable<IDependencyNode> sortedModules = null;
            IEnumerable<IDependencyNode> filteredModules = null;
            var timerNameBase = typeof(StartupHandler).FullName;

            // scan the assemblies for registered types for quick retrieval
            var scanSetup = objectFactory.CreateTimedTask();
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
            var moduleSortSetup = objectFactory.CreateTimedTask();
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
            var containerSetup = objectFactory.CreateTimedTask();
            containerSetup.Name = timerNameBase + ".ContainerSetup";
            containerSetup.TimedAction = () =>
            {
                if (!(Locator is ILocatorRegistry registry))
                    throw new NullLocatorException();

                var containerDefaults = objectFactory.CreateContainerDefaults();

                if (containerDefaults == null)
                    throw new NotSupportedException("Unable to set container defaults, the object factory returned a null service for it!");

                containerDefaults.Configure(registry, filteredModules, config, objectFactory);
                var locatorRegistries = (registry as ILocatorResolveConfigureModules)?.ResolveConfigureModules(filteredModules, config)
                                            ?? (registry as ILocator).GetAll<ILocatorConfigure>();

                foreach (var map in locatorRegistries ?? Enumerable.Empty<ILocatorConfigure>())
                {
                    map.Configure(registry, this);
                }

                var readOnlyLocator = Internal.ReadOnlyLocator.CreateReadOnlyLocator(registry as ILocator);
                registry.Add(typeof(ILocator), readOnlyLocator);

                tempContext = objectFactory.CreateStartupContext(readOnlyLocator, filteredModules, sortedModules, config);
                registry.Add(typeof(IStartupContext), tempContext);

                ImportHelper.OnEnsureLocator += (() => readOnlyLocator); // configure import<T> locator

                _OnLocatorStartupComplete?.Invoke(); //execute locator complete before verification since last minute additions can occur here.
                _LocatorStartupInvoked = true;
                (registry as ILocatorVerification)?.Verify();
            };

            // startup modules
            var startupModulesTask = objectFactory.CreateTimedTask();
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
            var delayedStart = config as IStartupDelayed;
            void startup() => config.TimedTaskManager.Execute(startupModulesTask);

            if (delayedStart?.EnableDelayedStartup == true)
            {
                delayedStart.DelayedStartup = startup;
            }
            else
            {
                startup();
            }

            // assign the context(s) after running tasks
            _Context = context = tempContext;

            _Started = true;

            return _Started;
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