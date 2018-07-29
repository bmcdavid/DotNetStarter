namespace DotNetStarter.Abstractions
{
    //todo: can this be removed?

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
    }
}
