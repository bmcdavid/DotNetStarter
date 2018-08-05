using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using LightInject;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// LightInject Registry
    /// </summary>
    public class LightInjectLocatorRegistry : ILocatorRegistry, ILocatorRegistryWithContains, ILocatorRegistryWithResolveConfigureModules, ILocatorRegistryWithRemove, ILocatorRegistryWithVerification
    {
        private IServiceContainer _container;
        private ContainerRegistrationCollection _registrations;
        private bool _verified;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceContainer"></param>
        /// <param name="containerRegistrations"></param>
        public LightInjectLocatorRegistry(IServiceContainer serviceContainer, ContainerRegistrationCollection containerRegistrations)
        {
            _container = serviceContainer;
            _registrations = containerRegistrations;
            _verified = false;
        }

        /// <summary>
        /// Access to IServiceContainer
        /// </summary>
        public object InternalContainer => _container;

        /// <summary>
        /// Adds service type for service implementation
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifeTime = Lifecycle.Transient)
        {
            AddRegistration(new ContainerRegistration()
            {
                ServiceKey = key,
                Lifecycle = lifeTime,
                ServiceType = serviceType,
                ServiceImplementation = serviceImplementation
            });
        }

        /// <summary>
        /// Adds a service type by delegate
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifeTime)
        {
            AddRegistration(new ContainerRegistration()
            {
                Lifecycle = lifeTime,
                ServiceType = serviceType,
                ServiceFactory = implementationFactory
            });
        }

        /// <summary>
        /// Adds instance for service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            AddRegistration(new ContainerRegistration()
            {
                Lifecycle = Lifecycle.Singleton,
                ServiceType = serviceType,
                ServiceImplementation = serviceInstance?.GetType(),
                ServiceInstance = serviceInstance
            });
        }

        /// <summary>
        /// Adds TService
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        public void Add<TService, TImpl>(string key = null, Lifecycle lifetime = Lifecycle.Transient) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime);
        }

        /// <summary>
        /// Determines if container has a serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null)
        {
            return _verified ?
                _container.CanGetInstance(serviceType, ConvertKey(key)) : // only from container once Registrations are set
                _registrations.ContainsKey(serviceType);
        }

        /// <summary>
        /// Removes a registered item
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            _registrations.Remove(serviceType);
        }

        /// <summary>
        /// Creates IlocatorConfigure modules via Activator.CreateIntance
        /// </summary>
        /// <param name="filteredModules"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        IEnumerable<ILocatorConfigure> ILocatorRegistryWithResolveConfigureModules.ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration config)
        {
            if (_registrations.TryGetValue(typeof(ILocatorConfigure), out List<ContainerRegistration> locatorConfTypes))
            {
                foreach (var module in locatorConfTypes)
                {
                    if (module.ServiceInstance != null)
                    {
                        yield return (ILocatorConfigure)module.ServiceInstance;
                    }
                    else
                    {
                        yield return (ILocatorConfigure)Activator.CreateInstance(module.ServiceImplementation);
                    }
                }
            }
        }

        void ILocatorRegistryWithVerification.Verify()
        {
            foreach (var item in _registrations)
            {
                var count = item.Value.Count;

                for (int i = 0; i < count; i++)
                {
                    ContainerRegistration registration = item.Value[i];

                    // for GetAllInstances to work they must be named, except for last
                    string serviceKey = registration.ServiceKey ?? ((i + 1 == count) ? "" : nameof(DotNetStarter) + $"_{i}");

                    if (registration.ServiceInstance != null)
                    {
                        _container.RegisterInstance(registration.ServiceType, registration.ServiceInstance);
                    }
                    else if (registration.ServiceFactory != null)
                    {
                        _container.RegisterFallback((type, key) => type == registration.ServiceType, r => registration.ServiceFactory.Invoke(_container.GetInstance<ILocatorAmbient>().Current), ConvertLifetime(registration.Lifecycle));
                    }
                    else
                    {
                        _container.Register(registration.ServiceType, registration.ServiceImplementation, serviceKey, ConvertLifetime(registration.Lifecycle));
                    }
                }
            }

            _verified = true;
        }

        private void AddRegistration(ContainerRegistration registration)
        {
            var service = registration.ServiceType;
            var implementation = registration.ServiceInstance?.GetType() ?? registration.ServiceImplementation;
            RegistryExtensions.ConfirmService(service, implementation);

            if (!_registrations.TryGetValue(registration.ServiceType, out List<ContainerRegistration> storedTypes))
            {
                storedTypes = new List<ContainerRegistration>();
            }

            storedTypes.Add(registration);
            _registrations[registration.ServiceType] = storedTypes;
        }

        private string ConvertKey(string key) => key ?? string.Empty;

        private ILifetime ConvertLifetime(Lifecycle lifetime)
        {
            switch (lifetime)
            {
                case Lifecycle.Scoped:
                    return new PerScopeLifetime();

                case Lifecycle.Singleton:
                    return new PerContainerLifetime();

                case Lifecycle.Transient:
                    return null; // todo: understand PerRequestLifeTime();
                    // if not null, cannot pass args to a func<arg,tresult> factory
                    // System.IndexOutOfRangeException: 'Index was outside the bounds of the array.' is thrown
                    //return new PerRequestLifeTime();
            }

            return null;
        }
    }
}