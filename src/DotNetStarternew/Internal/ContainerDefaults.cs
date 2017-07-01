namespace DotNetStarter.Internal
{
    using Abstractions;
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
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="configuration"></param>
        /// <param name="objectFactory"></param>
        public virtual void Configure(ILocatorRegistry locator, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration configuration, IStartupObjectFactory objectFactory)
        {
            Type initModuleType = typeof(IStartupModule);
            Type configureModuleType = typeof(ILocatorConfigure);
            var modules = filteredModules.Select(x => x.Node).OfType<Type>();

            foreach (var module in modules)
            {
                if (initModuleType.IsAssignableFromCheck(module))
                    locator.Add(initModuleType, module, null, LifeTime.Singleton, ConstructorType.Greediest);

                if (configureModuleType.IsAssignableFromCheck(module))
                    locator.Add(configureModuleType, module, null, LifeTime.Singleton, ConstructorType.Greediest);
            }

            // add default instances    
            locator.Add(typeof(ILocator), locator);
            locator.Add(typeof(IStartupConfiguration), configuration);
            locator.Add(typeof(IStartupObjectFactory), objectFactory);
            locator.Add(typeof(IStartupLogger), configuration.Logger);
            locator.Add(typeof(IAssemblyScanner), configuration.AssemblyScanner);
            locator.Add(typeof(IDependencyFinder), configuration.DependencyFinder);
            locator.Add(typeof(IDependencySorter), configuration.DependencySorter);
            locator.Add<ITimedTask, TimedTask>(lifetime: LifeTime.Transient, constructorType: ConstructorType.Greediest);

            ImportHelper.OnEnsureLocator += (() => locator); // configure import<T> locator
        }
    }
}