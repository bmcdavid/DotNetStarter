using DotNetStarter.Abstractions;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Default startup configuration with web environment
    /// </summary>
    public class StartupConfigurationWithWebEnvironment : StartupConfiguration, IStartupConfigurationWithEnvironment<IStartupEnvironmentWeb>
    {
        private readonly IStartupEnvironmentWeb _Environment;

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
        public StartupConfigurationWithWebEnvironment
            (
                IEnumerable<Assembly> assemblies,
                IStartupEnvironmentWeb environment,
                IAssemblyFilter assemblyFilter,
                IAssemblyScanner assemblyScanner,
                IDependencyFinder dependencyFinder,
                IDependencySorter dependencySorter,
                IStartupLogger startupLogger,
                IStartupModuleFilter startupModuleFilter,
                ITimedTaskManager timedTaskManager
            )
            : base
            ( 
                assemblies,
                assemblyFilter,
                assemblyScanner,
                dependencyFinder,
                dependencySorter,
                startupLogger,
                startupModuleFilter,
                timedTaskManager,
                environment
            )
        {
            _Environment = environment;
        }

        /// <summary>
        /// Startup web environment
        /// </summary>
        public virtual new IStartupEnvironmentWeb Environment => _Environment;

        IStartupEnvironmentWeb IStartupConfigurationWithEnvironment<IStartupEnvironmentWeb>.Environment => _Environment;
    }
}
