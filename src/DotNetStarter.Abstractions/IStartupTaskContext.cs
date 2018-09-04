namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides data across startup tasks
    /// </summary>
    public interface IStartupTaskContext
    {
        /// <summary>
        /// IStartupConfiguration instance
        /// </summary>
        IStartupConfiguration Configuration { get; }
        
        /// <summary>
        /// Determines if Import&lt;T> is assigned
        /// </summary>
        bool EnableImport { get; }

        /// <summary>
        /// ILocator instance
        /// </summary>
        ILocator Locator { get; }

        /// <summary>
        /// ILocatorRegistry instance
        /// </summary>
        ILocatorRegistry LocatorRegistry { get; }
        
        /// <summary>
        /// Gets a task item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>();

        /// <summary>
        /// Sets a task item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemInstance"></param>
        void SetItem<T>(T itemInstance);
    }
}