namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default startup handler
    /// </summary>
    public class StartupHandler : IStartupHandler
    {
        internal static IStartupContext _Context;

        internal static IEnumerable<IStartupModule> _StartupModules;

        internal static IStartupEngine _Engine;

        /// <summary>
        /// Fires after IContainerConfigure.Configure has completed in all executing modules
        /// </summary>
        public event Action OnLocatorStartupComplete;

        /// <summary>
        /// Fires after IStartupModule.Startup has completed in all executing modules
        /// </summary>
        public event Action OnStartupComplete;

        // todo: verify this works in a website context and console....
        /// <summary>
        /// Finalizer
        /// </summary>        
        ~StartupHandler()
        {
            Dispose();
        }

        /// <summary>
        /// Context Reference
        /// </summary>
        protected IStartupContext Context => _Context;

        /// <summary>
        /// Modules reference
        /// </summary>
        protected IEnumerable<IStartupModule> InitModules => _StartupModules;

        /// <summary>
        /// Configuration Reference
        /// </summary>
        public IStartupConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Locator Reference
        /// </summary>
        public ILocator Locator { get; protected set; }

        /// <summary>
        /// Starup process, by default it scans assemblies, sorts modules, configures container, and runs startup for each module
        /// </summary>
        /// <param name="config"></param>
        /// <param name="objectFactory"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Startup(IStartupConfiguration config, IStartupObjectFactory objectFactory, out IStartupContext context)
        {
            _Engine = this;
            Configuration = config;
            Locator = objectFactory.CreateRegistry(config);

            IEnumerable<Assembly> assemblies = config.Assemblies;
            IStartupContext tempContext = null;
            IEnumerable<IDependencyNode> sortedModules = null;
            IEnumerable<IDependencyNode> filteredModules = null;
            IEnumerable<IStartupModule> modules = null;
            var timerNameBase = typeof(StartupHandler).FullName;

            // scan the assemblies for registered types for quick retrieval
            var scanSetup = objectFactory.CreateTimedTask();
            scanSetup.Name = timerNameBase + ".AssemblyScan";
            scanSetup.TimedAction = () =>
            {
                var registeredScanAttrs = assemblies.SelectMany(x => x.CustomAttribute(typeof(ScanTypeRegistryAttribute), false).OfType<ScanTypeRegistryAttribute>());
                var registerdScanTypes = registeredScanAttrs.SelectMany(x => x.ScanTypes);

                config.AssemblyScanner.Scan(assemblies, registerdScanTypes, config.AssemblyFilter.FilterAssembly);
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
                var registry = Locator as ILocatorRegistry;

                if (registry != null)
                {
                    var setDefaults = objectFactory.CreateContainerDefaults();

                    if (setDefaults == null)
                        throw new NotSupportedException("Unable to set container defaults, the object factory returned a null service for it!");

                    objectFactory.CreateContainerDefaults().Configure(registry, filteredModules, config, objectFactory);
                    //modules = registry.GetAll<IStartupModule>();
                    var locatorRegistries = registry.GetAll<ILocatorConfigure>();// modules.OfType<ILocatorConfigure>();

                    foreach (var map in locatorRegistries)
                    {
                        map.Configure(registry, this);
                    }
                }

                tempContext = objectFactory.CreateStartupContext(registry, filteredModules, sortedModules, config);

                // register context once its created
                registry?.Add(typeof(IStartupContext), tempContext);
                modules = registry?.GetAll<IStartupModule>(); // resolve all startup modules for DI
            };

            // execute tasks in order
            config.TimedTaskManager.Execute(scanSetup);
            config.TimedTaskManager.Execute(moduleSortSetup);
            config.TimedTaskManager.Execute(containerSetup);

            OnLocatorStartupComplete?.Invoke();

            // assign the context(s) after running tasks
            _Context = context = tempContext;

            // run startup calls, allows for container to be null
            Startup(modules ?? filteredModules.Select(x => Activator.CreateInstance(x.Node as Type)).OfType<IStartupModule>());
            OnStartupComplete?.Invoke();

            return true;
        }

        /// <summary>
        /// Executes on finalizer
        /// </summary>
        internal static void Shutdown()
        {
            if (_StartupModules != null)
            {
                foreach (var module in _StartupModules) //todo: should the order be reversed so ones with more dependencies shutdown before ones that have none
                {
                    try
                    {
                        module?.Shutdown(_Engine);
                    }
                    catch (Exception ex)
                    {
                        _Context.Configuration.Logger.
                            LogException($"Failed to shutdown module {module.GetType().FullName}!", ex, typeof(StartupHandler), LogLevel.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Startups up given modules
        /// </summary>
        /// <param name="modules"></param>
        protected virtual void Startup(IEnumerable<IStartupModule> modules)
        {
            _StartupModules = modules ?? Enumerable.Empty<IStartupModule>();

            foreach (var x in _StartupModules)
            {
                x.Startup(this);
            }

            // AppDomain.CurrentDomain.DomainUnload += (sender, e) => Shutdown();// netframework
            // AssemblyLoadContext.Unloading //netstandard1.6
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Shutdown();
                _StartupModules = null;
            }
        }
    }
}