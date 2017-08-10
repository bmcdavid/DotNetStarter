using DotNetStarter.Abstractions;

[assembly: LocatorRegistryFactory(typeof(DotNetStarter.StructureMapFactory))]

namespace DotNetStarter
{
    /// <summary>
    /// Locator with Structuremap Container 
    /// </summary>
    public class StructureMapFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates Structuremap Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new StructureMapLocator();
    }
}