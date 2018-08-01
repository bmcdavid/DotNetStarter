namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// startup Configuration
    /// </summary>
    public interface IStartupConfiguration
    {
        /// <summary>
        /// Assemblies used for startup process, may not be all application assemblies
        /// </summary>
        IEnumerable<Assembly> Assemblies { get; }

        /// <summary>
        /// Assembly filter
        /// </summary>
        IAssemblyFilter AssemblyFilter { get; }

        /// <summary>
        /// Assembly scanner
        /// </summary>
        IAssemblyScanner AssemblyScanner { get; }

        /// <summary>
        /// Dependency finder
        /// </summary>
        IDependencyFinder DependencyFinder { get; }

        /// <summary>
        /// Dependency sorter
        /// </summary>
        IDependencySorter DependencySorter { get; }

        /// <summary>
        /// Startup Environment
        /// </summary>
        IStartupEnvironment Environment { get; }

        /// <summary>
        /// Startup logger
        /// </summary>
        IStartupLogger Logger { get; }

        /// <summary>
        /// Module filter
        /// </summary>
        IStartupModuleFilter ModuleFilter { get; }

        /// <summary>
        /// Timed task manager
        /// </summary>
        ITimedTaskManager TimedTaskManager { get; }

        /// <summary>
        /// Registration lifecycle modifier
        /// </summary>
        IRegistrationLifecycleModifier RegistrationLifecycleModifier { get; }
    }
}