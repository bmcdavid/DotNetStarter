namespace DotNetStarter.Internal
{
    using Abstractions;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Default implementation for startup configuration objects.
    /// </summary>
    public class StartupConfiguration : IStartupConfiguration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="assemblyFilter"></param>
        /// <param name="assemblyScanner"></param>
        /// <param name="dependencyFinder"></param>
        /// <param name="dependencySorter"></param>
        /// <param name="logger"></param>
        /// <param name="moduleFilter"></param>
        /// <param name="timedTaskManager"></param>
        public StartupConfiguration(
            IEnumerable<Assembly> assemblies,
            IAssemblyFilter assemblyFilter,            
            IAssemblyScanner assemblyScanner,
            IDependencyFinder dependencyFinder,
            IDependencySorter dependencySorter, 
            IStartupLogger logger,
            IStartupModuleFilter moduleFilter,
            ITimedTaskManager timedTaskManager)
        {
            Assemblies = assemblies;
            AssemblyFilter = assemblyFilter;            
            AssemblyScanner = assemblyScanner;
            DependencyFinder = dependencyFinder;
            DependencySorter = dependencySorter;
            Logger = logger;
            ModuleFilter = moduleFilter;
            TimedTaskManager = timedTaskManager;
        }

        /// <summary>
        /// Application Assemblies
        /// </summary>
        public IEnumerable<Assembly> Assemblies { get; }

        /// <summary>
        /// Default assembly filter
        /// </summary>
        public IAssemblyFilter AssemblyFilter { get; }

        /// <summary>
        /// Default assembly scanner
        /// </summary>
        public IAssemblyScanner AssemblyScanner { get; }

        /// <summary>
        /// Default dependency finder
        /// </summary>
        public IDependencyFinder DependencyFinder { get; }

        /// <summary>
        /// Default dependency sorter
        /// </summary>
        public IDependencySorter DependencySorter { get; }

        /// <summary>
        /// Default logger
        /// </summary>
        public IStartupLogger Logger { get; }

        /// <summary>
        /// Default module filter
        /// </summary>
        public IStartupModuleFilter ModuleFilter { get; }

        /// <summary>
        /// Default timed task manager
        /// </summary>
        public ITimedTaskManager TimedTaskManager { get; }
    }
}
