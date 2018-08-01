using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using LightInject;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Creates LightInject ILocator
    /// </summary>
    public sealed class LightInjectLocatorRegistryFactory : ILocatorRegistryFactory
    {
        private readonly IServiceContainer _container;
        private ContainerRegistrationCollection _registrations;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LightInjectLocatorRegistryFactory()
        {
            _container = new ServiceContainer
            (
                new ContainerOptions()
                {
                    EnablePropertyInjection = false, // for netcore support
                }
            );
            _registrations = new ContainerRegistrationCollection();
        }

        /// <summary>
        /// Constructor with provided container
        /// </summary>
        /// <param name="container"></param>
        public LightInjectLocatorRegistryFactory(IServiceContainer container)
        {
            _container = container;
            _registrations = new ContainerRegistrationCollection();
        }

        /// <summary>
        /// Creates LightInject Locator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => new LightInjectLocator(_container, _registrations);

        /// <summary>
        /// Creates default LightInject ILocatorRegistry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new LightInjectLocatorRegistry(_container, _registrations);
    }
}
