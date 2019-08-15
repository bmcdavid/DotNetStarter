namespace DotNetStarter.Internal
{
    using Abstractions;
    using Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Assigns default instances to the container
    /// </summary>
    public class ContainerDefaults : ILocatorDefaultRegistrations
    {
        private static readonly Type LocatorConfigureType = typeof(ILocatorConfigure);
        private static readonly Type StartupModuleType = typeof(IStartupModule);

        /// <summary>
        /// ILocatorConfigure modules added during configuration
        /// </summary>
        public ILocatorConfigureModuleCollection LocatorConfigureModuleCollection { get; set; }

        /// <summary>
        /// IStartupModules added during configuration
        /// </summary>
        public IStartupModuleCollection StartupModuleCollection { get; set; }

        /// <summary>
        /// Assigns default instances to the locator
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="filteredModules"></param>
        /// <param name="configuration"></param>
        public virtual void Configure(ILocatorRegistry registry, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration configuration)
        {
            var modules = filteredModules.Select(x => x.Node).OfType<Type>();
            RegisterScannedModules(registry, modules);
            RegisterStartupModuleCollection(registry);
            RegisterLocatorConfigureCollection(registry);

            // add default instances
            registry
                .AddInstance(configuration)
                .AddInstance(configuration.Environment)
                .AddInstance(configuration.Logger)
                .AddInstance(configuration.AssemblyScanner)
                .AddInstance(configuration.DependencyFinder)
                .AddInstance(configuration.DependencySorter)
                .AddTransient<ITimedTask, TimedTask>();
        }

        /// <summary>
        /// Registers canned modules
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="modules"></param>
        protected static void RegisterScannedModules(ILocatorRegistry registry, IEnumerable<Type> modules)
        {
            foreach (var module in modules)
            {
                if (StartupModuleType.IsAssignableFromCheck(module))
                    registry.Add(StartupModuleType, module, null, Lifecycle.Singleton);

                if (LocatorConfigureType.IsAssignableFromCheck(module))
                    registry.Add(LocatorConfigureType, module, null, Lifecycle.Singleton);
            }
        }

        /// <summary>
        /// Registers ILocatorConfigure modules
        /// </summary>
        /// <param name="registry"></param>
        protected void RegisterLocatorConfigureCollection(ILocatorRegistry registry)
        {
            if (LocatorConfigureModuleCollection?.Count > 0)
            {
                foreach (var module in LocatorConfigureModuleCollection)
                {
                    registry.Add(LocatorConfigureType, module);
                }
            }
        }

        /// <summary>
        /// Registers IStartupModules
        /// </summary>
        /// <param name="registry"></param>
        protected void RegisterStartupModuleCollection(ILocatorRegistry registry)
        {
            if (!(StartupModuleCollection?.Count > 0)) { return; }

            foreach (var module in StartupModuleCollection)
            {
                if (module.ModuleInstance is object)
                {
                    registry.Add(StartupModuleType, module.ModuleInstance);
                    continue;
                }

                if (module.ModuleType is object)
                {
                    registry.Add(StartupModuleType, module.ModuleType, lifecycle: Lifecycle.Singleton);
                    continue;
                }
            }
        }
    }
}