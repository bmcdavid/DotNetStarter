using DotNetStarter.Abstractions;
using StructureMap;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Locator with Structuremap Container
    /// </summary>
    public class StructureMapFactory : ILocatorRegistryFactory
    {
        private readonly IContainer _container;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StructureMapFactory() : this(null) { }

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public StructureMapFactory(IContainer container) => _container = container;

        /// <summary>
        /// Creates Structuremap Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new StructureMapLocator(_container);
    }
}