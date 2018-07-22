namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DryIoc;

    /// <summary>
    /// Locator with DryIoc Container
    /// </summary>
    public class DryIocLocatorFactory : ILocatorRegistryFactory
    {
        private readonly IContainer _container;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DryIocLocatorFactory() : this(null) { }

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public DryIocLocatorFactory(IContainer container) => _container = container;

        /// <summary>
        /// Creates DryIoc Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new DryIocLocator(_container);
    }
}