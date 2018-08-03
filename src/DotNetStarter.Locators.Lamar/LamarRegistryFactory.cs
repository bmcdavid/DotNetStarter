using DotNetStarter.Abstractions;
using Lamar;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Locator with Lamar Container
    /// </summary>
    public class LamarLocatorRegistryFactory : ILocatorRegistryFactory
    {
        private readonly Container _container;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LamarLocatorRegistryFactory() => _container = new Container(r => { });

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public LamarLocatorRegistryFactory(Container container) => _container = container;

        /// <summary>
        /// Creates Lamar Locator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => new LamarLocator(_container);

        /// <summary>
        /// Creates Lamar LocatorRegistry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new LamarLocatorRegistry(_container);
    }
}