using DotNetStarter.Abstractions;

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