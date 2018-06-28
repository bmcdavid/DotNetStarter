using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Default LightInject ILocatoryRegistry
    /// </summary>
    public class LightInjectLocator : ILocatorRegistry, ILocator, ILocatorVerification, ILocatorCreateScope,
        ILocatorRegistryWithContains, ILocatorResolveConfigureModules, ILocatorRegistryWithRemove,
        ILocatorSetContainer
    {
        private IServiceContainer _Container;
        private ContainerRegistrationCollection _Registrations;
        private bool _Verified;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceContainer"></param>
        public LightInjectLocator(IServiceContainer serviceContainer = null)
        {
            _Container = serviceContainer ?? new ServiceContainer
                (
                    new ContainerOptions()
                    {
                        EnablePropertyInjection = false, // for netcore support
                    }
                );
            _Registrations = new ContainerRegistrationCollection();
            _Verified = false;
        }

        /// <summary>
        /// DebugInfo
        /// </summary>
        public string DebugInfo => _Registrations.DebugInformation();

        /// <summary>
        /// Access to IServiceContainer
        /// </summary>
        public object InternalContainer => _Container;

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
            return _Verified ?
                _Container.CanGetInstance(serviceType, ConvertKey(key)) : // only from container once Registrations are set
                _Registrations.ContainsKey(serviceType);
        }

        /// <summary>
        /// Creates an ILocatorScoped
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope()
        {
            return new LightInjectLocatorScoped
            (
                _Container.BeginScope(),
                this
            );
        }

        /// <summary>
        /// Disposes IServiceContainer
        /// </summary>
        public void Dispose()
        {
            // hack: disposing throws StackOverFlowException, so its not done
            //_Container.Dispose();
        }

        /// <summary>
        /// Gets service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null)
        {
            return _Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null)
        {
            return _Container.GetInstance<T>();
        }

        /// <summary>
        /// Gets all T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            var list = _Container.GetAllInstances<T>();
            SortList(ref list, typeof(T));

            return list;
        }

        /// <summary>
        /// Gets all serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            var list = _Container.GetAllInstances(serviceType);
            SortList(ref list, serviceType);

            return list;
        }

        /// <summary>
        /// Removes a registered item
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            _Registrations.Remove(serviceType);
        }

        /// <summary>
        /// Creates IlocatorConfigure modules via Activator.CreateIntance
        /// </summary>
        /// <param name="filteredModules"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public IEnumerable<ILocatorConfigure> ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration config)
        {
            if (_Registrations.TryGetValue(typeof(ILocatorConfigure), out List<ContainerRegistration> locatorConfTypes))
            {
                foreach (var module in locatorConfTypes)
                {
                    if (module.ServiceInstance != null)
                        yield return (ILocatorConfigure)module.ServiceInstance;

                    yield return (ILocatorConfigure)Activator.CreateInstance(module.ServiceImplementation);
                }
            }
        }

        /// <summary>
        /// Allows container to be set externally, example is ConfigureServices in a netcore app
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(object container)
        {
            var tempContainer = container as IServiceContainer;
            _Container = tempContainer ?? throw new ArgumentException($"{container} doesn't implement {typeof(IServiceContainer).FullName}!");
        }

        /// <summary>
        /// Converts registration dictionary to actual container registration.
        /// </summary>
        public void Verify()
        {
            foreach (var item in _Registrations)
            {
                var count = item.Value.Count;

                for (int i = 0; i < count; i++)
                {
                    ContainerRegistration registration = item.Value[i];

                    // for GetAllInstances to work they must be named, except for last
                    string serviceKey = registration.ServiceKey ?? ((i + 1 == count) ? "" : nameof(DotNetStarter) + $"_{i}");

                    if (registration.ServiceInstance != null)
                    {
                        _Container.RegisterInstance(registration.ServiceType, registration.ServiceInstance);
                    }
                    else if (registration.ServiceFactory != null)
                    {
                        _Container.RegisterFallback((type, key) => type == registration.ServiceType, r => registration.ServiceFactory.Invoke(this), ConvertLifetime(registration.Lifecycle));
                    }
                    else
                    {
                        _Container.Register(registration.ServiceType, registration.ServiceImplementation, serviceKey, ConvertLifetime(registration.Lifecycle));
                    }
                }
            }

            _Verified = true;
        }

        private static void ThrowRegisterException(Type service, Type implementation)
        {
            var ex = new ArgumentException($"{implementation.FullName} cannot be converted to {service.FullName}!");

            throw ex;
        }

        private void AddRegistration(ContainerRegistration registration)
        {
            var service = registration.ServiceType;
            var implementation = registration.ServiceInstance?.GetType() ?? registration.ServiceImplementation;

            if (implementation != null && !service.IsAssignableFromCheck(implementation))
            {
                if (!service.IsGenericType())
                {
                    ThrowRegisterException(service, implementation);
                }
                else
                {
                    if (!implementation.IsGenericInterface(service))
                    {
                        ThrowRegisterException(service, implementation);
                    }
                }
            }

            if (!_Registrations.TryGetValue(registration.ServiceType, out List<ContainerRegistration> storedTypes))
            {
                storedTypes = new List<ContainerRegistration>();
            }

            storedTypes.Add(registration);
            _Registrations[registration.ServiceType] = storedTypes;
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
                    //return new PerRequestLifeTime();
            }

            return null;
        }

        // hack: GetAllInstances Requires additional sorting as LightInject returns from ConcurrentDictionary
        // required for LightInject, which causes many other considerations as constructor injection isn't supported by this, affects DotNetStarter.Web injection
        /* IEnumerable will need another sorting mechanism for most things
         * I will need to work around it in IStartupModule cases like in DotNetStarter.Web
         * this also won't support delegate rolutions
         */
        private void SortList<T>(ref IEnumerable<T> list, Type service)
        {
            List<ContainerRegistration> sourceList;

            if (_Registrations.TryGetValue(service, out sourceList))
            {
                var typed = from o in sourceList.Select(x => x.ServiceImplementation)
                            join i in list
                            on o equals i.GetType()
                            select i;

                list = typed.ToList();
            };
        }
    }
}
