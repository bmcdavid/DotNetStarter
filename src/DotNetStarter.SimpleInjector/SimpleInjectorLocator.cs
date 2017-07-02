namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using SimpleInjector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Creates a locator based on SimpleInjector
    /// </summary>
    public class SimpleInjectorLocator : ILocatorRegistry, ILocatorSetContainer, ILocatorResolveConfigureModules, ILocatorVerification
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected Container _Container;

        private static ContainerRegistrations _Registrations = new ContainerRegistrations();

        private ContainerRegistrations _ScopedRegistrations;

        private bool _RootContainer;

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleInjectorLocator(Container container = null)
        {
            _RootContainer = container == null;
            _Container = container ?? new Container();
            _ScopedRegistrations = new ContainerRegistrations();
            //_Container.Options.AllowOverridingRegistrations = true;
        }

        /// <summary>
        /// Provides debug information about the container
        /// </summary>
        public string DebugInfo => _Registrations.DebugInformation();

        /// <summary>
        /// Allows access to wrapped container
        /// </summary>
        public virtual object InternalContainer => _Container;

        /// <summary>
        /// Adds service type to container, given its implementation type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        /// <param name="constructorType"></param>
        public virtual void Add(Type serviceType, Type serviceImplementation, string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest)
        {
            AddRegistration(new ContainerRegistration()
            {
                LifeTime = lifetime,
                ServiceType = serviceType,
                ServiceImplementation = serviceImplementation
            });
        }

        /// <summary>
        /// Adds service type to container, given its implementation type via generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        /// <param name="constructorType"></param>
        public virtual void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest)
            where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime, constructorType);
        }

        /// <summary>
        /// Addes service type to container given a factory to create the type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public virtual void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime)
        {
            AddRegistration(new ContainerRegistration()
            {
                LifeTime = lifeTime,
                ServiceType = serviceType,
                ServiceFactory = implementationFactory
            });
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            AddRegistration(new ContainerRegistration()
            {
                LifeTime = LifeTime.Singleton,
                ServiceType = serviceType,
                ServiceInstance = serviceInstance
            });
        }

        /// <summary>
        /// Builds up properties of given object, useful in webforms.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool BuildUp(object target)
        {
            // http://simpleinjector.readthedocs.io/en/latest/decisions.html#no-out-of-the-box-support-for-property-injection
            try
            {
                var properties = target.GetType().GetPropertiesCheck();

                properties.All(x =>
                   {
                       object v;

                       if (x.PropertyType.IsAssignableFromCheck(typeof(IEnumerable)))
                       {
                           v = GetAll(x.PropertyType); //todo: handle resolving generics?
                       }
                       else
                       {
                           v = Get(x.PropertyType);
                       }

                       x.SetValue(target, v, null);

                       return true;
                   });
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if container has service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsService(Type serviceType, string key = null)
        {
            return _Registrations.ContainsKey(serviceType);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            // _Container?.Dispose(); //todo: disposing is bad since there is only 1
        }

        /// <summary>
        /// Gets service instance given type and optional key
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type service, string key = null)
        {
            try
            {
                return _Container.GetInstance(service);
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Gets service instance given type and optional key for generics
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TService Get<TService>(string key = null) => (TService)Get(typeof(TService), key);

        /// <summary>
        /// Gets all registered services. Key is optional.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            try
            {
                return _Container.GetAllInstances(serviceType);
            }
            catch (Exception) { }

            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Gets all registered services for generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<TService> GetAll<TService>(string key = null) => GetAll(typeof(TService)).OfType<TService>();

        /// <summary>
        /// Creates a scoped container
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            // https://github.com/simpleinjector/SimpleInjector/issues/18
            // todo: how will this handle adding scoped registrations from Web package
            return new SimpleInjectorLocator(_Container); // Simple Injector has implicit scoping??
        }

        /// <summary>
        /// Removes a service, note: not all containers support this.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public virtual void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            _Registrations.Remove(serviceType);
        }

        /// <summary>
        /// Simple Injector cannot resolve anything during setup, it locks the container, so we must manually create them
        /// </summary>
        /// <param name="filteredModules"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public IEnumerable<ILocatorConfigure> ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration config)
        {
            if (_Registrations.TryGetValue(typeof(ILocatorConfigure), out HashSet<ContainerRegistration> locatorConfTypes))
            {
                foreach (var module in locatorConfTypes)
                {
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
            var tempContainer = container as Container;
            _Container = tempContainer ?? throw new ArgumentException($"{container} doesn't implement {typeof(Container).FullName}!");
        }

        /// <summary>
        /// Converts registration dictionary to actual container registration.
        /// </summary>
        public void Verify()
        {
            foreach (var item in _Registrations)
            {
                if (item.Value.Any(x => x.ServiceImplementation != null))
                {
                    //todo: how to register collection items with a lifestyle
                    //var itemsToRegister = item.Value.Select(x => ConvertToRegistration(x.ServiceType, x.ServiceImplementation, x.LifeTime));
                    //_Container.RegisterCollection(itemsToRegister);
                    _Container.RegisterCollection(item.Key, item.Value.Select(x => x.ServiceImplementation));
                }

                var registration = item.Value.Last();

                if (registration.ServiceInstance != null)
                {
                    _Container.Register(registration.ServiceType, () => registration.ServiceInstance, ConvertLifeTime(registration.LifeTime));
                }
                else if (registration.ServiceFactory != null)
                {
                    _Container.Register(registration.ServiceType, () => registration.ServiceFactory(this), ConvertLifeTime(registration.LifeTime));
                }
                else
                {
                    // below checks for open generic
                    //if (registration.ServiceType.IsGenericTypeDefinitionCheck())
                    //{
                    //    _Container.Register(registration.ServiceType, item.Value.
                    //        Where(x => !x.ServiceImplementation.IsGenericTypeDefinitionCheck()).
                    //        Select(x => x.ServiceImplementation), ConvertLifeTime(registration.LifeTime));
                    //}
                    //else
                    {
                        _Container.Register(registration.ServiceType, registration.ServiceImplementation, ConvertLifeTime(registration.LifeTime));
                    }
                }
            }

            //todo: can we call verify when complete? currently throws exceptions
            //  _Container.Verify();
        }

        private static Lifestyle ConvertLifeTime(LifeTime lifetime)
        {
            switch (lifetime)
            {
                case LifeTime.Singleton:
                    return Lifestyle.Singleton;

                case LifeTime.Transient:
                    return Lifestyle.Transient;

                case LifeTime.Scoped:
                    return Lifestyle.Scoped;

                case LifeTime.HttpRequest:
                    return Lifestyle.Scoped;

                case LifeTime.AlwaysUnique:
                    return Lifestyle.Transient;
            }

            return Lifestyle.Transient;
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

            var registry = _RootContainer ? _Registrations : _ScopedRegistrations;

            if (!registry.TryGetValue(registration.ServiceType, out HashSet<ContainerRegistration> storedTypes))
            {
                storedTypes = new HashSet<ContainerRegistration>();
            }

            storedTypes.Add(registration);
            registry[registration.ServiceType] = storedTypes;
        }
        private Registration ConvertToRegistration(Type service, Type serviceImpl, LifeTime lifetime)
        {
            switch (lifetime)
            {
                case LifeTime.Singleton:
                    return Lifestyle.Singleton.CreateRegistration(serviceImpl, _Container);

                case LifeTime.Scoped:
                    return Lifestyle.Scoped.CreateRegistration(serviceImpl, _Container);

                case LifeTime.Transient:
                case LifeTime.AlwaysUnique:
                default:
                    return Lifestyle.Transient.CreateRegistration(serviceImpl, _Container);
            }
        }
    }
}