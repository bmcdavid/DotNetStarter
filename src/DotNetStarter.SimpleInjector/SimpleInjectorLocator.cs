[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.SimpleInjectorLocatorFactory))]

namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using SimpleInjector;
    using SimpleInjector.Advanced;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class ContainerRegistration
    {
        public Type ServiceType { get; set; }

        public Type ServiceImplementation { get; set; }

        public object ServiceInstance { get; set; }

        public LifeTime LifeTime { get; set; }

        public Func<ILocator, object> ServiceFactory { get; set; }


    }

    internal class ContainerRegistrations : Dictionary<Type, HashSet<ContainerRegistration>>
    {
        public string DebugInformation()
        {
            return string.Join(",", this.Select(x => x.Key));//todo, build better output
        }
    }

    /// <summary>
    /// Creates a locator based on DryIoc.dll
    /// </summary>
    public class SimpleInjectorLocator : ILocatorRegistry, ILocatorSetContainer, ILocatorResolveConfigureModules, ILocatorVerification
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected Container _Container;

        static ContainerRegistrations _Registrations = new ContainerRegistrations();

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleInjectorLocator(Container container = null)
        {
            _Container = container ?? new Container();
            _Container.Options.AllowOverridingRegistrations = true;
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
            HashSet<ContainerRegistration> storedTypes;

            if (!_Registrations.TryGetValue(serviceType, out storedTypes))
            {
                storedTypes = new HashSet<ContainerRegistration>();
            }

            storedTypes.Add(new ContainerRegistration()
            {
                LifeTime = lifetime,
                ServiceType = serviceType,
                ServiceImplementation = serviceImplementation
            });

            _Registrations[serviceType] = storedTypes;
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
            HashSet<ContainerRegistration> storedTypes;

            if (!_Registrations.TryGetValue(serviceType, out storedTypes))
            {
                storedTypes = new HashSet<ContainerRegistration>();
            }

            storedTypes.Add(new ContainerRegistration()
            {
                LifeTime = lifeTime,
                ServiceType = serviceType,
                ServiceFactory = implementationFactory
            });

            _Registrations[serviceType] = storedTypes;

            // _Container.Register(serviceType, () => implementationFactory(this), ConvertLifeTime(lifeTime));
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            HashSet<ContainerRegistration> storedTypes;

            if (!_Registrations.TryGetValue(serviceType, out storedTypes))
            {
                storedTypes = new HashSet<ContainerRegistration>();
            }

            storedTypes.Add(new ContainerRegistration()
            {
                LifeTime = LifeTime.Singleton,
                ServiceType = serviceType,
                ServiceInstance = serviceInstance
            });

            _Registrations[serviceType] = storedTypes;
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
                           v = GetAll(x.PropertyType); //todo: handle generics

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
            try
            {
                return _Container.GetInstance(serviceType) != null;
            }
            catch (Exception)
            {
                return false;
            }
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
            catch
            {
            }

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
        /// Allows container to be set externally, example is ConfigureServices in a netcore app
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(object container)
        {
            var tempContainer = container as Container;
            _Container = tempContainer ?? throw new ArgumentException($"{container} doesn't implement {typeof(Container).FullName}!");
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

        public IEnumerable<ILocatorConfigure> ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules)
        {
            Type configureModuleType = typeof(ILocatorConfigure);
            var modules = filteredModules.Select(x => x.Node).OfType<Type>();

            foreach (var module in modules)
            {
                if (configureModuleType.IsAssignableFromCheck(module))
                    yield return (ILocatorConfigure)Activator.CreateInstance(module);
            }
        }

        public void Verify()
        {
            foreach (var item in _Registrations)
            {
                //var registrationItems = _Registrations.SelectMany(x => x.Value).Select(y => ConvertToRegistration(y.ServiceType, y.ServiceImplementation, y.LifeTime));
                //_Container.RegisterCollection(registrationItems);

                if (item.Value.Any(x => x.ServiceImplementation != null))
                {
                    _Container.RegisterCollection(item.Value.Select(x => ConvertToRegistration(x.ServiceType, x.ServiceImplementation, x.LifeTime)));
                    //_Container.RegisterCollection(item.Key, item.Value.Select(x => x.ServiceImplementation));
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
                    _Container.Register(registration.ServiceType, registration.ServiceImplementation, ConvertLifeTime(registration.LifeTime));
                }

            }
            //todo: iterate registrations
            //todo: how to handle enumerables
            //container.RegisterCollection(typeof(ILogger), assemblies);

            //container.AppendToCollection(typeof(ILogger), typeof(ExtraLogger));
            //// Registration
            //if (_StoredTypes.Contains(serviceType))
            //{
            //    _Container.AppendToCollection(serviceType, serviceImplementation);
            //}
            //else
            //{
            //    _Container.RegisterCollection(serviceType, new Type[] { serviceImplementation });
            //    _StoredTypes.Add(serviceType);
            //}

            //_Container.Register(serviceType, serviceImplementation, ConvertLifeTime(lifetime));
            //_Container.Register()
            // _Container.Verify();
        }
    }

    /// <summary>
    /// Locator with SimpleInjector Container 
    /// </summary>
    public class SimpleInjectorLocatorFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates SimpleInjector Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new SimpleInjectorLocator();
    }
}