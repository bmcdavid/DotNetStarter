using DotNetStarter.Abstractions;
using StructureMap;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Locator with Structuremap Container
    /// </summary>
    public class StructureMapSignedFactory : ILocatorRegistryFactory
    {
        private readonly IContainer _container;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StructureMapSignedFactory() => _container = new Container();

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public StructureMapSignedFactory(IContainer container) => _container = container;

        /// <summary>
        /// Creates Structuremap Locator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => new StructureMapSignedLocator(_container);

        /// <summary>
        /// Creates Structuremap LocatorRegistry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new StructureMapSignedLocatorRegistry(_container);
    }
}