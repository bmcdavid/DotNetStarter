using DotNetStarter.Abstractions;
using Stashbox;
using System;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Stashbox Locator Registry
    /// </summary>
    public class StashboxRegistry : ILocatorRegistry, ILocatorRegistryWithContains
    {
        private readonly IStashboxContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public StashboxRegistry(IStashboxContainer container) => _container = container;

        /// <summary>
        /// Stashbox Container
        /// </summary>
        public object InternalContainer => _container;

        /// <summary>
        /// Adds serviceImplementation as serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifecycle = Lifecycle.Transient) => CommonAdd(serviceType, serviceImplementation, lifecycle);

        /// <summary>
        /// Adds with implementation factory
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifecycle"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifecycle) => _container.Register
        (
            serviceType,
            c => c
            .ConvertLifetime(lifecycle)
            .WithFactory(r => implementationFactory(r.Resolve<ILocatorAmbient>().Current))
        );

        /// <summary>
        /// Adds service instance for service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance) => _container.RegisterInstance(serviceType, serviceInstance);

        /// <summary>
        /// Adds TImpl as TService
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        public void Add<TService, TImpl>(string key = null, Lifecycle lifecycle = Lifecycle.Transient) where TImpl : TService => CommonAdd(typeof(TService), typeof(TImpl), lifecycle, true);

        /// <summary>
        /// Determines if service is available
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null) => _container.CanResolve(serviceType);

        private void CommonAdd(Type serviceType, Type serviceImplementation, Lifecycle lifecycle, bool isGeneric = false)
        {
            if (!isGeneric) { RegistryExtensions.ConfirmService(serviceType, serviceImplementation); }

            _container.Register(serviceType, serviceImplementation, c => c.ConvertLifetime(lifecycle));
        }
    }
}