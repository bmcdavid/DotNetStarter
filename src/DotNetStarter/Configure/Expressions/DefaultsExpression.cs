using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Configure.Expressions
{
    /// <summary>
    /// Allows ability to override default service instances
    /// </summary>
    public sealed class DefaultsExpression
    {
        internal IAssemblyFilter AssemblyFilter { get; private set; }
        internal IAssemblyScanner AssemblyScanner { get; private set; }
        internal ILocatorDefaultRegistrations ContainerDefaults { get; private set; }
        internal IDependencyFinder DependencyFinder { get; private set; }
        internal IDependencySorter DependencySorter { get; private set; }
        internal IStartupLogger Logger { get; private set; }
        internal IRegistrationsModifier RegistrationModifier { get; private set; }
        internal ILocatorRegistryFactory RegistryFactory { get; private set; }
        internal Action<ILocatorRegistry> RegistryFinalizer { get; private set; }
        internal Func<IRequestSettingsProvider> RequestSettingsProviderFactory { get; private set; }
        internal Func<IStartupConfiguration, IStartupHandler> StartupHandlerFactory { get; private set; }
        internal Func<ITimedTask> TimedTaskFactory { get; private set; }
        internal ITimedTaskManager TimedTaskManager { get; private set; }

        /// <summary>
        /// Sets the startup IAssemblyFilter
        /// </summary>
        /// <param name="assemblyFilter"></param>
        /// <returns></returns>
        public DefaultsExpression UseAssemblyFilter(IAssemblyFilter assemblyFilter)
        {
            AssemblyFilter = assemblyFilter ?? throw new ArgumentNullException(nameof(assemblyFilter));
            return this;
        }

        /// <summary>
        /// Sets the startup IAssemblyScanner
        /// </summary>
        /// <param name="assemblyScanner"></param>
        /// <returns></returns>
        public DefaultsExpression UseAssemblyScanner(IAssemblyScanner assemblyScanner)
        {
            AssemblyScanner = assemblyScanner ?? throw new ArgumentNullException(nameof(assemblyScanner));
            return this;
        }

        /// <summary>
        /// CAUTION: Advanced usage to override the default registrations, do so with extreme caution.
        /// </summary>
        /// <param name="locatorDefaultRegistrations"></param>
        /// <returns></returns>
        public DefaultsExpression UseContainerDefaults(ILocatorDefaultRegistrations locatorDefaultRegistrations)
        {
            ContainerDefaults = locatorDefaultRegistrations ?? throw new ArgumentNullException(nameof(locatorDefaultRegistrations));
            return this;
        }

        /// <summary>
        /// Sets the startup IDependencyFinder
        /// </summary>
        /// <param name="dependencyFinder"></param>
        /// <returns></returns>
        public DefaultsExpression UseDependencyFinder(IDependencyFinder dependencyFinder)
        {
            DependencyFinder = dependencyFinder ?? throw new ArgumentNullException(nameof(dependencyFinder));
            return this;
        }

        /// <summary>
        /// Sets the startup IDependencySorter
        /// </summary>
        /// <param name="dependencySorter"></param>
        /// <returns></returns>
        public DefaultsExpression UseDependencySorter(IDependencySorter dependencySorter)
        {
            DependencySorter = dependencySorter ?? throw new ArgumentNullException(nameof(dependencySorter));
            return this;
        }

        /// <summary>
        /// Sets the startup ILocatorRegistryFactory, bypassing assembly resolutions
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public DefaultsExpression UseLocatorRegistryFactory(ILocatorRegistryFactory factory)
        {
            RegistryFactory = factory;
            return this;
        }

        /// <summary>
        /// Sets the startup IStartupLogger
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public DefaultsExpression UseLogger(IStartupLogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return this;
        }

        /// <summary>
        /// Allows ability to customize a registration's lifecycle set in the attribute
        /// </summary>
        /// <param name="registrationModifier"></param>
        /// <returns></returns>
        public DefaultsExpression UseRegistrationModifier(IRegistrationsModifier registrationModifier)
        {
            RegistrationModifier = registrationModifier;
            return this;
        }

        /// <summary>
        /// Allows application developers ability to make final adjustments to registry during configuration
        /// </summary>
        /// <param name="registryFinalizer"></param>
        /// <returns></returns>
        public DefaultsExpression UseRegistryFinalizer(Action<ILocatorRegistry> registryFinalizer)
        {
            RegistryFinalizer = registryFinalizer;
            return this;
        }

        /// <summary>
        /// Creates the request settings provider for the default ITimedTaskManager implementation
        /// <para>Note: This call is not used when ITimedTaskManager is overriden!</para>
        /// </summary>
        /// <param name="requestSettingsProviderFactory"></param>
        /// <returns></returns>
        public DefaultsExpression UseRequestSettingsProviderFactory(Func<IRequestSettingsProvider> requestSettingsProviderFactory)
        {
            RequestSettingsProviderFactory = requestSettingsProviderFactory ?? throw new ArgumentNullException(nameof(requestSettingsProviderFactory));
            return this;
        }

        /// <summary>
        /// CAUTION: Advanced usage to override the startup handler, do so with extreme caution.
        /// </summary>
        /// <param name="startupHandlerFactory"></param>
        /// <returns></returns>
        public DefaultsExpression UseStartupHandler(Func<IStartupConfiguration, IStartupHandler> startupHandlerFactory)
        {
            StartupHandlerFactory = startupHandlerFactory ?? throw new ArgumentNullException(nameof(startupHandlerFactory));
            return this;
        }

        /// <summary>
        /// Creates timed tasks
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        /// <returns></returns>
        public DefaultsExpression UseTimedTaskFactory(Func<ITimedTask> timedTaskFactory)
        {
            TimedTaskFactory = timedTaskFactory;
            return this;
        }

        /// <summary>
        /// Sets the startup ITimedTaskManager
        /// </summary>
        /// <param name="timedTaskManager"></param>
        /// <returns></returns>
        public DefaultsExpression UseTimedTaskManager(ITimedTaskManager timedTaskManager)
        {
            TimedTaskManager = timedTaskManager ?? throw new ArgumentNullException(nameof(timedTaskManager));
            return this;
        }
    }
}