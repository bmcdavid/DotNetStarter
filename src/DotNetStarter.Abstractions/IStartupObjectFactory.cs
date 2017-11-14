namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Implementations must have empty constructor!
    /// </summary>
    public interface IStartupObjectFactory
    {
        /// <summary>
        /// Highest number gets picked as configuration, be sure all factories return valid implementations
        /// </summary>
        int SortOrder { get; }

        /// <summary>
        /// Creates assembly filter
        /// </summary>
        /// <returns></returns>
        IAssemblyFilter CreateAssemblyFilter();

        /// <summary>
        /// Creates assembly scanners
        /// </summary>
        /// <returns></returns>
        IAssemblyScanner CreateAssemblyScanner();

        /// <summary>
        /// Creates container default configuration object
        /// </summary>
        /// <returns></returns>
        ILocatorDefaultRegistrations CreateContainerDefaults();

        /// <summary>
        /// Creates dependency finders.
        /// </summary>
        /// <returns></returns>
        IDependencyFinder CreateDependencyFinder();

        /// <summary>
        /// Creates dependency nodes, which need constructors that take same arguments as this factory.
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        IDependencyNode CreateDependencyNode(object nodeType, Type attributeType);

        /// <summary>
        /// Create dependency node sorter
        /// </summary>
        /// <returns></returns>
        IDependencySorter CreateDependencySorter();

        /// <summary>
        /// Creates initalization configuration object
        /// </summary>
        /// <returns></returns>
        IStartupConfiguration CreateStartupConfiguration(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Creates the startup context object
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="allModules"></param>
        /// <param name="startupConfiguration"></param>
        /// <returns></returns>
        IStartupContext CreateStartupContext(IReadOnlyLocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> allModules, IStartupConfiguration startupConfiguration);

        /// <summary>
        /// Creates a logger
        /// </summary>
        /// <returns></returns>
        IStartupLogger CreateStartupLogger();

        /// <summary>
        /// Create startup handler
        /// </summary>
        /// <returns></returns>
        IStartupHandler CreateStartupHandler();
        
        /// <summary>
        /// Creates a module filter
        /// </summary>
        /// <returns></returns>
        IStartupModuleFilter CreateModuleFilter();

        /// <summary>
        /// Creates instances of locator registries.
        /// </summary>
        /// <returns></returns>
        ILocatorRegistry CreateRegistry(IStartupConfiguration startupConfiguration);

        /// <summary>
        /// Creates a wrapper for Http Item dictionary and 
        /// </summary>
        /// <returns></returns>
        IRequestSettingsProvider CreateRequestSettingsProvider();

        /// <summary>
        /// Creates instances of timed tasks
        /// </summary>
        /// <returns></returns>
        ITimedTask CreateTimedTask();

        /// <summary>
        /// Creates instances of timed task managers
        /// </summary>
        /// <returns></returns>
        ITimedTaskManager CreateTimedTaskManager();
    }
}