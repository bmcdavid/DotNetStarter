using DotNetStarter.Abstractions;
using Grace.DependencyInjection;
using System;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Grace Locator Registry
    /// </summary>
    public class GraceLocatorRegistry : ILocatorRegistry, ILocatorRegistryWithContains
    {
        private DependencyInjectionContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public GraceLocatorRegistry(DependencyInjectionContainer container) => _container = container;

        /// <summary>
        /// Grace Container
        /// </summary>
        public object InternalContainer => _container;

        /// <summary>
        /// Adds service type to container, given its implementation type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifecycle = Lifecycle.Transient)
        {
            RegistryExtensions.ConfirmService(serviceType, serviceImplementation);
            _container.Configure(c =>
            {
                c.Export(serviceImplementation)
                .As(serviceType)
                .ConfigureLifetime(lifecycle);
            });
        }

        /// <summary>
        /// Adds service type to container given a factory to create the type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifecycle"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifecycle)
        {
            _container.Configure(c =>
            {
                c.ExportFactory(() => implementationFactory(_container.Locate<ILocatorAmbient>().Current))
                .As(serviceType)
                .ConfigureLifetime(lifecycle);
            });
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _container.Configure(c =>
            {
                c.ExportInstance(serviceInstance)
                .As(serviceType)
                .ConfigureLifetime(Lifecycle.Singleton);
            });
        }

        /// <summary>
        /// Adds service type to container, given its implementation type via generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        public void Add<TService, TImpl>(string key = null, Lifecycle lifecycle = Lifecycle.Transient) where TImpl : TService
        {
            _container.Configure(c =>
            {
                c.Export(typeof(TImpl))
                .As(typeof(TService))
                .ConfigureLifetime(lifecycle);
            });
        }

        /// <summary>
        /// Determines if service is registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null) => _container.CanLocate(serviceType, key: key);
    }
}