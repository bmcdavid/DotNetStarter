namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Creates an ILocatorRegistry
    /// </summary>
    public interface ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates an ILocatoryRegistry
        /// </summary>
        /// <returns></returns>
        ILocatorRegistry CreateRegistry();

        /// <summary>
        /// Creates an ILocator
        /// </summary>
        /// <returns></returns>
        ILocator CreateLocator();
    }
}