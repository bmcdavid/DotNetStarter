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

    /// <summary>
    /// Verifies container when complete
    /// </summary>
    [StartupModule]
    public class SimpleInjectorVerification : ILocatorConfigure
    {
        /// <summary>
        /// Verifies container when complete
        /// </summary>
        /// <param name="container"></param>
        /// <param name="engine"></param>
        public void Configure(ILocatorRegistry container, IStartupEngine engine)
        {
            engine.OnLocatorStartupComplete += () =>
            {
                var x = (container as SimpleInjectorLocator).InternalContainer;
                //(x as Container).Verify(); // todo: fix so verfication works
            };
        }
    }

    /// <summary>
    /// Creates a locator based on DryIoc.dll
    /// </summary>
    public class SimpleInjectorLocator : ILocatorRegistry, ILocatorSetContainer, ILocatorResolveConfigureModules
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected Container _Container;

        static HashSet<Type> _StoredTypes = new HashSet<Type>();

        static HashSet<Type> _RemovedTypes = new HashSet<Type>();

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
        public string DebugInfo => null; //todo: how do i provide debug info?

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
            //todo: how to handle enumerables
            //container.RegisterCollection(typeof(ILogger), assemblies);

            //container.AppendToCollection(typeof(ILogger), typeof(ExtraLogger));
            // Registration
            if (_StoredTypes.Contains(serviceType))
            {
                _Container.AppendToCollection(serviceType, serviceImplementation);
            }
            else
            {
                _Container.RegisterCollection(serviceType, new Type[] { serviceImplementation });
                _StoredTypes.Add(serviceType);
            }

            _Container.Register(serviceType, serviceImplementation, ConvertLifeTime(lifetime));
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
            _Container.Register(serviceType, () => implementationFactory(this), ConvertLifeTime(lifeTime));
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _Container.Register(serviceType, () => serviceInstance, ConvertLifeTime(LifeTime.Singleton));
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
            if (!IsRemoved(service))
            {
                return _Container.GetInstance(service);
            }

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
                if (!IsRemoved(serviceType))
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
            _RemovedTypes.Add(serviceType);
            // throw new NotImplementedException(); // can anything be done here?
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

        private static bool IsRemoved(Type t)
        {
            return _RemovedTypes.Contains(t);
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