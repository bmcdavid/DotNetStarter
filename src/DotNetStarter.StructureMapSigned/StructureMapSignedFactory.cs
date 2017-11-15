using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Locator with Structuremap Container
    /// </summary>
    public class StructureMapSignedFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates Structuremap Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new StructureMapSignedLocator();
    }
}