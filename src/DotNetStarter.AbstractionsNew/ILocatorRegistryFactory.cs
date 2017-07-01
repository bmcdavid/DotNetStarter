namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Creates locator registry objects
    /// </summary>
    public interface ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates locator registry objects
        /// </summary>
        /// <returns></returns>
        ILocatorRegistry CreateRegistry();
    }
}
