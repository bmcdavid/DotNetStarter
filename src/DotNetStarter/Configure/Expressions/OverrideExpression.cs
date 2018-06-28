using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Configure.Expressions
{
    /// <summary>
    /// Allows ability to override default service instances
    /// </summary>
    public sealed class OverrideExpression
    {
        internal IAssemblyFilter AssemblyFilter { get; private set; }
        internal IAssemblyScanner AssemblyScanner { get; private set; }
        internal ILocatorDefaultRegistrations ContainerDefaults { get; private set; }
        internal IDependencyFinder DependencyFinder { get; private set; }
        internal IDependencySorter DependencySorter { get; private set; }
        internal IStartupLogger Logger { get; private set; }
        internal ILocatorRegistryFactory RegistryFactory { get; private set; }
        internal Func<IRequestSettingsProvider> RequestSettingsProviderFactory { get; private set; }
        internal IStartupHandler StartupHandler { get; private set; }
        internal Func<ITimedTask> TimedTaskFactory { get; private set; }
        internal ITimedTaskManager TimedTaskManager { get; private set; }

        /// <summary>
        /// Sets the startup IAssemblyFilter
        /// </summary>
        /// <param name="assemblyFilter"></param>
        /// <returns></returns>
        public OverrideExpression UseAssemblyFilter(IAssemblyFilter assemblyFilter)
        {
            AssemblyFilter = assemblyFilter ?? throw new ArgumentNullException(nameof(assemblyFilter));
            return this;
        }

        /// <summary>
        /// Sets the startup IAssemblyScanner
        /// </summary>
        /// <param name="assemblyScanner"></param>
        /// <returns></returns>
        public OverrideExpression UseAssemblyScanner(IAssemblyScanner assemblyScanner)
        {
            AssemblyScanner = assemblyScanner ?? throw new ArgumentNullException(nameof(assemblyScanner));
            return this;
        }

        /// <summary>
        /// CAUTION: Advanced usage to override the default registrations, do so with extreme caution.
        /// </summary>
        /// <param name="locatorDefaultRegistrations"></param>
        /// <returns></returns>
        public OverrideExpression UseContainerDefaults(ILocatorDefaultRegistrations locatorDefaultRegistrations)
        {
            ContainerDefaults = locatorDefaultRegistrations ?? throw new ArgumentNullException(nameof(locatorDefaultRegistrations));
            return this;
        }

        /// <summary>
        /// Sets the startup IDependencyFinder
        /// </summary>
        /// <param name="dependencyFinder"></param>
        /// <returns></returns>
        public OverrideExpression UseDependencyFinder(IDependencyFinder dependencyFinder)
        {
            DependencyFinder = dependencyFinder ?? throw new ArgumentNullException(nameof(dependencyFinder));
            return this;
        }

        /// <summary>
        /// Sets the startup IDependencySorter
        /// </summary>
        /// <param name="dependencySorter"></param>
        /// <returns></returns>
        public OverrideExpression UseDependencySorter(IDependencySorter dependencySorter)
        {
            DependencySorter = dependencySorter ?? throw new ArgumentNullException(nameof(dependencySorter));
            return this;
        }

        /// <summary>
        /// Sets the startup ILocatorRegistryFactory, bypassing assembly resolutions
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public OverrideExpression UseLocatorRegistryFactory(ILocatorRegistryFactory factory)
        {
            RegistryFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            return this;
        }

        /// <summary>
        /// Sets the startup IStartupLogger
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public OverrideExpression UseLogger(IStartupLogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return this;
        }

        /// <summary>
        /// Creates the request settings provider for the default ITimedTaskManager implementation
        /// <para>Note: This call is not used when ITimedTaskManager is overriden!</para>
        /// </summary>
        /// <param name="requestSettingsProviderFactory"></param>
        /// <returns></returns>
        public OverrideExpression UseRequestSettingsProviderFactory(Func<IRequestSettingsProvider> requestSettingsProviderFactory)
        {
            RequestSettingsProviderFactory = requestSettingsProviderFactory ?? throw new ArgumentNullException(nameof(requestSettingsProviderFactory));
            return this;
        }

        /// <summary>
        /// CAUTION: Advanced usage to override the startup handler, do so with extreme caution.
        /// </summary>
        /// <param name="startupHandler"></param>
        /// <returns></returns>
        public OverrideExpression UseStartupHandler(IStartupHandler startupHandler)
        {
            StartupHandler = startupHandler ?? throw new ArgumentNullException(nameof(startupHandler));
            return this;
        }

        /// <summary>
        /// Creates timed tasks
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        /// <returns></returns>
        public OverrideExpression UseTimedTaskFactory(Func<ITimedTask> timedTaskFactory)
        {
            TimedTaskFactory = timedTaskFactory;
            return this;
        }

        /// <summary>
        /// Sets the startup ITimedTaskManager
        /// </summary>
        /// <param name="timedTaskManager"></param>
        /// <returns></returns>
        public OverrideExpression UseTimedTaskManager(ITimedTaskManager timedTaskManager)
        {
            TimedTaskManager = timedTaskManager ?? throw new ArgumentNullException(nameof(timedTaskManager));
            return this;
        }
    }
}