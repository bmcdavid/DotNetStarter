namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Lamar Locator
    /// </summary>
    public class LamarLocatorRegistry : ILocatorRegistry, ILocatorRegistryWithContains, ILocatorRegistryWithRemove, ILocatorRegistryWithVerification, ILocatorRegistryWithResolveConfigureModules
    {
        /// <summary>
        /// Lamar container
        /// </summary>
        private readonly IContainer _container;

        private readonly IServiceCollection _serviceDescriptors = new ServiceRegistry();

        /// <summary>
        /// Constructor
        /// </summary>
        public LamarLocatorRegistry(IContainer container) => _container = container;

        /// <summary>
        /// Raw Lamar container
        /// </summary>
        public virtual object InternalContainer => _container;

        /// <summary>
        /// Add object instance
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance) => _serviceDescriptors.AddSingleton(serviceType, serviceInstance);

        /// <summary>
        /// Add by delegate
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifeTime) => _serviceDescriptors.Add
        (
            new ServiceDescriptor
            (
                serviceType,
                provider => implementationFactory(_container.GetService<ILocatorAmbient>().Current),
                ConvertLifetime(lifeTime)
            )
        );

        /// <summary>
        /// Add by type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifeTime = Lifecycle.Transient)
        {
            ConfirmService(serviceType, serviceImplementation);
            _serviceDescriptors.Add(new ServiceDescriptor(serviceType, serviceImplementation, ConvertLifetime(lifeTime)));
        }

        /// <summary>
        /// Add by generic
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        public void Add<TService, TImpl>(string key = null, Lifecycle lifetime = Lifecycle.Transient) where TImpl : TService => _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImpl), ConvertLifetime(lifetime)));

        /// <summary>
        /// Checks if service is registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null) => _serviceDescriptors.Any(x => x.ServiceType == serviceType);

        /// <summary>
        /// Remove service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null) => _serviceDescriptors.RemoveAll(serviceType);

        IEnumerable<ILocatorConfigure> ILocatorRegistryWithResolveConfigureModules.ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration startupConfiguration)
        {
            foreach (var module in _serviceDescriptors.Where(x => x.ServiceType == typeof(ILocatorConfigure)).ToList())
            {
                if (module.ImplementationInstance != null)
                    yield return (ILocatorConfigure)module.ImplementationInstance;
                else
                    yield return (ILocatorConfigure)Activator.CreateInstance(module.ImplementationType);
            }
        }

        void ILocatorRegistryWithVerification.Verify() => _container.Configure(_serviceDescriptors);

        private static void ConfirmService(Type serviceType, Type serviceImplementation)
        {
            if (!serviceType.IsAssignableFromCheck(serviceImplementation))
            {
                if (!serviceType.IsGenericType())
                {
                    ThrowRegisterException(serviceType, serviceImplementation);
                }
                else
                {
                    if (!serviceImplementation.IsGenericInterface(serviceType))
                    {
                        ThrowRegisterException(serviceType, serviceImplementation);
                    }
                }
            }
        }

        private static void ThrowRegisterException(Type service, Type implementation)
        {
            var ex = new ArgumentException($"{implementation.FullName} cannot be converted to {service.FullName}!");

            throw ex;
        }

        private ServiceLifetime ConvertLifetime(Lifecycle lifeTime)
        {
            switch (lifeTime)
            {
                case Lifecycle.Scoped: return ServiceLifetime.Scoped;
                case Lifecycle.Singleton: return ServiceLifetime.Singleton;
                default: return ServiceLifetime.Transient;
            }
        }
    }
}