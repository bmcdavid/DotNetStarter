using DotNetStarter.Abstractions;
using Stashbox;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Creates a Stashbox Locator
    /// </summary>
    public class StashboxLocatorRegistryFactory : ILocatorRegistryFactory
    {
        private readonly IStashboxContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        public StashboxLocatorRegistryFactory() => _container = new StashboxContainer();

        /// <summary>
        /// Constructor with container provided
        /// </summary>
        /// <param name="container"></param>
        public StashboxLocatorRegistryFactory(IStashboxContainer container) => _container = container;

        /// <summary>
        /// Creates a Stashbox Locator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => new StashboxLocator(_container);

        /// <summary>
        /// Creates a Stashbox Registry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new StashboxRegistry(_container);
    }
}