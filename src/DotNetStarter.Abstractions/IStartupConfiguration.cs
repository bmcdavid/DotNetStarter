namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;
    using System.Reflection;

    //todo: v2, add reference to IStartupEnvironment

    /// <summary>
    /// startup Configuration
    /// </summary>
    public interface IStartupConfiguration
    {
        /// <summary>
        /// Application References for startup
        /// </summary>
        IEnumerable<Assembly> Assemblies { get; }

        /// <summary>
        /// Default assembly filter useful for AssemblyScanner
        /// </summary>
        IAssemblyFilter AssemblyFilter { get; }

        /// <summary>
        /// Default assembly scanner
        /// </summary>
        IAssemblyScanner AssemblyScanner { get; }

        /// <summary>
        /// Default dependency finder
        /// </summary>
        IDependencyFinder DependencyFinder { get; }

        /// <summary>
        /// Default dependency sorter
        /// </summary>
        IDependencySorter DependencySorter { get; }

        /// <summary>
        /// Default startup logger
        /// </summary>
        IStartupLogger Logger { get; }

        /// <summary>
        /// Default module filter
        /// </summary>
        IStartupModuleFilter ModuleFilter { get; }

        /// <summary>
        /// Default timed task manager
        /// </summary>
        ITimedTaskManager TimedTaskManager { get; }
    }
}