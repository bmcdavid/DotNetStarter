﻿namespace DotNetStarter.Internal
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Assigns default instances to the container
    /// </summary>
    public class ContainerDefaults : ILocatorDefaultRegistrations
    {
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
                    registry.Add(initModuleType, module, null, LifeTime.Singleton, ConstructorType.Greediest);

                if (configureModuleType.IsAssignableFromCheck(module))
                    registry.Add(configureModuleType, module, null, LifeTime.Singleton, ConstructorType.Greediest);
            }

            // add default instances    
            registry.Add(typeof(IStartupConfiguration), configuration);
            registry.Add(typeof(IStartupObjectFactory), objectFactory);
            registry.Add(typeof(IStartupLogger), configuration.Logger);
            registry.Add(typeof(IAssemblyScanner), configuration.AssemblyScanner);
            registry.Add(typeof(IDependencyFinder), configuration.DependencyFinder);
            registry.Add(typeof(IDependencySorter), configuration.DependencySorter);
            registry.Add<ITimedTask, TimedTask>(lifetime: LifeTime.Transient, constructorType: ConstructorType.Greediest);    
        }
    }
}