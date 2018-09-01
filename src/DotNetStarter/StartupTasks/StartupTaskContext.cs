using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Provides data across startup tasks
    /// </summary>
    public sealed class StartupTaskContext
    {
        private readonly IItemCollection _taskItems;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="enableImport"></param>
        /// <param name="locatorRegistryFactory"></param>
        /// <param name="startupConfiguration"></param>
        /// <param name="locatorDefaultRegistrations"></param>
        /// <param name="locatorRegistryFinalizer"></param>
        /// <param name="locatorConfigureEngine"></param>
        public StartupTaskContext(bool enableImport, ILocatorRegistryFactory locatorRegistryFactory, IStartupConfiguration startupConfiguration, ILocatorDefaultRegistrations locatorDefaultRegistrations, Action<ILocatorRegistry> locatorRegistryFinalizer, LocatorConfigureEngine locatorConfigureEngine)
        {
            EnableImport = enableImport;
            LocatorRegistry = locatorRegistryFactory?.CreateRegistry();
            Locator = locatorRegistryFactory?.CreateLocator();
            Configuration = startupConfiguration;
            _taskItems = new StartupEnvironmentItemCollection();
            _taskItems.Set(locatorDefaultRegistrations);
            _taskItems.Set(locatorRegistryFinalizer);
            _taskItems.Set(locatorConfigureEngine);
        }

        /// <summary>
        /// Determines if Import&lt;T> is assigned
        /// </summary>
        public bool EnableImport { get; }

        /// <summary>
        /// IStartupConfiguration instance
        /// </summary>
        public IStartupConfiguration Configuration { get; }

        /// <summary>
        /// ILocatorRegistry instance
        /// </summary>
        public ILocatorRegistry LocatorRegistry { get; }

        /// <summary>
        /// ILocator instance
        /// </summary>
        public ILocator Locator { get; }

        /// <summary>
        /// Sets a task item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemInstance"></param>
        public void SetItem<T>(T itemInstance) => _taskItems.Set(itemInstance);

        /// <summary>
        /// Gets a task item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() => _taskItems.Get<T>();
    }
}