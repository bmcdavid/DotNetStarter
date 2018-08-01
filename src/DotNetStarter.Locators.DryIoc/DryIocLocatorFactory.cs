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
        public DryIocLocatorFactory()
        {
            var rules = Rules.Default
                .WithoutThrowIfDependencyHasShorterReuseLifespan()
                .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                .WithTrackingDisposableTransients() //used in transient delegate cases
                ;

            _container = new Container(rules);
        }

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public DryIocLocatorFactory(IContainer container) => _container = container;

        /// <summary>
        /// Creates an ILocator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => new DryIocLocator(_container);

        /// <summary>
        /// Creates DryIoc Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new DryIocLocatorRegistry(_container);
    }
}