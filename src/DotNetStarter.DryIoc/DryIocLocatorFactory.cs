[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.DryIocLocatorFactory))]

namespace DotNetStarter
{
    using DotNetStarter.Abstractions;

    /// <summary>
    /// Locator with DryIoc Container 
    /// </summary>
    public class DryIocLocatorFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates DryIoc Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new DryIocLocator();
    }
}