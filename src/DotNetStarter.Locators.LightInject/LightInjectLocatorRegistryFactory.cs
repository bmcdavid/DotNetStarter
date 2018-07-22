using DotNetStarter.Abstractions;
using LightInject;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Creates LightInject ILocator
    /// </summary>
    public sealed class LightInjectLocatorRegistryFactory : ILocatorRegistryFactory
    {
        private readonly IServiceContainer _container;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LightInjectLocatorRegistryFactory() : this(null) { }

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public LightInjectLocatorRegistryFactory(IServiceContainer container) => _container = container;

        /// <summary>
        /// Creates default LightInject ILocator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new LightInjectLocator(_container);
    }
}
