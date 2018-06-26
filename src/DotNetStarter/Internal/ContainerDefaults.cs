namespace DotNetStarter.Internal
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IlocatorDefaultRegistrationsWithCollections
    {
        IStartupModuleCollection StartupModuleCollection { get; set; }
        ILocatorConfigureCollection LocatorConfigureModuleCollection { get; set; }
    }

    /// <summary>
    /// Assigns default instances to the container
    /// </summary>
    public class ContainerDefaults : ILocatorDefaultRegistrations, IlocatorDefaultRegistrationsWithCollections
    {
        public IStartupModuleCollection StartupModuleCollection { get; set; }
        public ILocatorConfigureCollection LocatorConfigureModuleCollection { get; set; }

        /// <summary>
        /// Assigns default instances to the locator
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="filteredModules"></param>
        /// <param name="configuration"></param>
        /// <param name="objectFactory"></param>
        public virtual void Configure(ILocatorRegistry registry, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration configuration, IStartupObjectFactory objectFactory)
        {
            Type initModuleType = typeof(IStartupModule);
            Type configureModuleType = typeof(ILocatorConfigure);
            var modules = filteredModules.Select(x => x.Node).OfType<Type>();

            foreach (var module in modules)
            {
                if (initModuleType.IsAssignableFromCheck(module))
                    registry.Add(initModuleType, module, null, Lifecycle.Singleton);

                if (configureModuleType.IsAssignableFromCheck(module))
                    registry.Add(configureModuleType, module, null, Lifecycle.Singleton);
            }

            if (StartupModuleCollection?.Count > 0)
            {
                foreach (var module in StartupModuleCollection)
                {
                    if (module.ModuleInstance != null)
                    {
                        registry.Add(initModuleType, module.ModuleInstance);
                    }
                    else if (module.ModuleType != null)
                    {
                        registry.Add(initModuleType, module.ModuleType, null, Lifecycle.Singleton);
                    }
                }
            }

            if (LocatorConfigureModuleCollection?.Count > 0)
            {
                foreach (var module in LocatorConfigureModuleCollection)
                {
                    registry.Add(configureModuleType, module);
                }
            }

            // add default instances    
            registry.Add(typeof(IStartupConfiguration), configuration);
            registry.Add(typeof(IStartupObjectFactory), objectFactory);
            registry.Add(typeof(IStartupLogger), configuration.Logger);
            registry.Add(typeof(IAssemblyScanner), configuration.AssemblyScanner);
            registry.Add(typeof(IDependencyFinder), configuration.DependencyFinder);
            registry.Add(typeof(IDependencySorter), configuration.DependencySorter);
            registry.Add<ITimedTask, TimedTask>(lifecycle: Lifecycle.Transient);
        }
    }
}