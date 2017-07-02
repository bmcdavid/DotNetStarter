using DotNetStarter.Abstractions;

//register this as the default configuration
[assembly: StartupObjectFactory(typeof(DotNetStarter.StartupObjectFactory))]

namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default initalization object factory, responsible for creating types before container is ready
    /// </summary>
    public class StartupObjectFactory : IStartupObjectFactory
    {
        /// <summary>
        /// Set to zero, if overriding set to higher number.
        /// </summary>
        public virtual int SortOrder => 0;

        /// <summary>
        /// Creates default assembly filter.
        /// </summary>
        /// <returns></returns>
        public IAssemblyFilter CreateAssemblyFilter() => new AssemblyFilter();

        /// <summary>
        /// Creates default assembly scanner.
        /// </summary>
        /// <returns></returns>
        public virtual IAssemblyScanner CreateAssemblyScanner() => new AssemblyScanner();

        /// <summary>
        /// Creates default container configuration
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorDefaultRegistrations CreateContainerDefaults() => new ContainerDefaults();

        /// <summary>
        /// Creates default dependency finder.
        /// </summary>
        /// <returns></returns>
        public virtual IDependencyFinder CreateDependencyFinder() => new DependencyFinder();

        /// <summary>
        /// Creates dependency nodes, used in dependency finder
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public virtual IDependencyNode CreateDependencyNode(object nodeType, Type attributeType) =>
            new DependencyNode(nodeType, attributeType);

        /// <summary>
        /// Creates default dependency sorter
        /// </summary>
        /// <returns></returns>
        public virtual IDependencySorter CreateDependencySorter() => new DependencySorter();

        /// <summary>
        /// Creates default startup configuration
        /// </summary>
        /// <param name="assemblies">Assemblies found from assembly loader.</param>
        /// <returns></returns>
        public virtual IStartupConfiguration CreateStartupConfiguration(IEnumerable<Assembly> assemblies) =>
            new StartupConfiguration(
                assemblies,
                CreateAssemblyFilter(),
                CreateAssemblyScanner(),
                CreateDependencyFinder(),
                CreateDependencySorter(),
                CreateStartupLogger(),
                CreateModuleFilter(),
                CreateTimedTaskManager()
            );

        /// <summary>
        /// Creates default startup context.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="allModules"></param>
        /// <param name="startupConfiguration"></param>
        /// <returns></returns>
        public virtual IStartupContext CreateStartupContext(ILocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> allModules, IStartupConfiguration startupConfiguration) =>
            new StartupContext(locator, allModules, filteredModules, startupConfiguration);

        /// <summary>
        /// Creates default initalization logger.
        /// </summary>
        /// <returns></returns>
        public virtual IStartupLogger CreateStartupLogger() => new StringLogger();

        /// <summary>
        /// Creates default initalization handler.
        /// </summary>
        /// <returns></returns>
        public virtual IStartupHandler CreateStartupHandler() => new StartupHandler();

        /// <summary>
        /// Creates default module filter.
        /// </summary>
        /// <returns></returns>
        public virtual IStartupModuleFilter CreateModuleFilter() => new StartupModuleFilter();

        /// <summary>
        /// Creates default ILocatorRegistry, default is null;
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorRegistry CreateRegistry(IStartupConfiguration config)
        {
            return GetFactory<LocatorRegistryFactoryAttribute, ILocatorRegistryFactory>(config)?.CreateRegistry();        
        }

        /// <summary>
        /// Creates default request settings provider, used for timed tasks with request scope.
        /// </summary>
        /// <returns></returns>
        public virtual IRequestSettingsProvider CreateRequestSettingsProvider() => new RequestSettingsProvider();

        /// <summary>
        /// Creates a timed task.
        /// </summary>
        /// <returns></returns>
        public virtual ITimedTask CreateTimedTask() => new TimedTask();

        /// <summary>
        /// Creates default timed task manager.
        /// </summary>
        /// <returns></returns>
        public virtual ITimedTaskManager CreateTimedTaskManager() => new TimedTaskManager();

        /// <summary>
        /// Gets factory type for given factory attribute
        /// </summary>
        /// <typeparam name="TFactoryAttr"></typeparam>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual TFactoryType GetFactory<TFactoryAttr,TFactoryType>(IStartupConfiguration config) where TFactoryAttr : AssemblyFactoryBaseAttribute
        {
            var dependents = config.DependencyFinder.Find<TFactoryAttr>(config.Assemblies);
            var sorted = config.DependencySorter.Sort<TFactoryAttr>(dependents);
            var attr = sorted.LastOrDefault()?.NodeAttribute as AssemblyFactoryBaseAttribute;

            if (attr == null)
                return default(TFactoryType);

            return (TFactoryType)Activator.CreateInstance(attr.FactoryType);
        }
    }
}