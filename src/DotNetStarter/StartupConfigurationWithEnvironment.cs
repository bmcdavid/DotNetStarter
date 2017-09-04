using DotNetStarter.Abstractions;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter
{
    /// <summary>
    /// Default startup configuration with environment
    /// </summary>
    public class StartupConfigurationWithEnvironment : IStartupConfigurationWithEnvironment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="environment"></param>
        /// <param name="assemblyFilter"></param>
        /// <param name="assemblyScanner"></param>
        /// <param name="dependencyFinder"></param>
        /// <param name="dependencySorter"></param>
        /// <param name="startupLogger"></param>
        /// <param name="startupModuleFilter"></param>
        /// <param name="timedTaskManager"></param>
        public StartupConfigurationWithEnvironment(
            IEnumerable<Assembly> assemblies,
            IStartupEnvironment environment,
            IAssemblyFilter assemblyFilter,
            IAssemblyScanner assemblyScanner,
            IDependencyFinder dependencyFinder,
            IDependencySorter dependencySorter,
            IStartupLogger startupLogger,
            IStartupModuleFilter startupModuleFilter,
            ITimedTaskManager timedTaskManager
        )
        {
            Environment = environment;
            Assemblies = assemblies;
            AssemblyFilter = assemblyFilter;
            AssemblyScanner = AssemblyScanner;
            DependencyFinder = dependencyFinder;
            DependencySorter = dependencySorter;
            Logger = startupLogger;
            ModuleFilter = startupModuleFilter;
            TimedTaskManager = timedTaskManager;
        }

        /// <summary>
        /// Startup environment reference
        /// </summary>
        public virtual IStartupEnvironment Environment { get; }

        /// <summary>
        /// Application References for startup
        /// </summary>
        public IEnumerable<Assembly> Assemblies { get; }

        /// <summary>
        /// Default assembly filter useful for AssemblyScanner
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
        /// Default startup logger
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
