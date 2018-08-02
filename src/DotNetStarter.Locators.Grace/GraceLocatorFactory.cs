using DotNetStarter.Abstractions;
using Grace.DependencyInjection;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Grace Locator Registry Factory
    /// </summary>
    public class GraceLocatorRegistryFactory : ILocatorRegistryFactory
    {
        private readonly DependencyInjectionContainer _container;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public GraceLocatorRegistryFactory() => _container = new DependencyInjectionContainer();

        /// <summary>
        /// Constructor with container provided
        /// </summary>
        /// <param name="container"></param>
        public GraceLocatorRegistryFactory(DependencyInjectionContainer container) => _container = container;

        /// <summary>
        /// Creates Grace ILocator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => new GraceLocator(_container);

        /// <summary>
        /// Creates Grace ILocatorRegistry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new GraceLocatorRegistry(_container);
    }
}
