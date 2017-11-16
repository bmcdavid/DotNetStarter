using DotNetStarter.Abstractions;
using LightInject;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Default LightInject ILocatoryRegistry
    /// </summary>
    public class LightInjectLocator : ILocatorRegistry, ILocatorCreateScope, ILocatorWithPropertyInjection, ILocatorRegistryWithContains
    {
        private IServiceContainer _Container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceContainer"></param>
        public LightInjectLocator(IServiceContainer serviceContainer = null)
        {
            _Container = serviceContainer ?? new ServiceContainer();
        }

        /// <summary>
        /// DebugInfo
        /// </summary>
        public string DebugInfo => "not supported";

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
        /// <param name="constructorType"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, LifeTime lifeTime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest)
        {
            _Container.Register(serviceType, serviceImplementation, ConvertAddKey(key, serviceType), ConvertLifetime(lifeTime));
        }

        /// <summary>
        /// Adds a service type by delegate
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime)
        {
            _Container.RegisterFallback((type, key) => type == serviceType, r => implementationFactory(this), ConvertLifetime(lifeTime));
        }

        /// <summary>
        /// Adds instance for service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _Container.RegisterInstance(serviceType, serviceInstance);
        }

        /// <summary>
        /// Adds TService
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        /// <param name="constructorType"></param>
        public void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest) where TImpl : TService
        {
            _Container.Register<TService, TImpl>(ConvertAddKey(key, typeof(TService)), ConvertLifetime(lifetime));
        }

        /// <summary>
        /// Injects properties
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BuildUp(object target)
        {
            var r = _Container.InjectProperties(target);

            return r != null;
        }

        /// <summary>
        /// Determines if container has a serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null)
        {
            return _Container.CanGetInstance(serviceType, ConvertKey(key));
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
            _Container.Dispose();
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
            return _Container.GetInstance<IEnumerable<T>>();
        }

        /// <summary>
        /// Gets all serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Container.GetAllInstances(serviceType);
        }

        private ILifetime ConvertLifetime(LifeTime lifetime)
        {
            switch (lifetime)
            {
                case LifeTime.Scoped:
                    return new PerScopeLifetime();
                case LifeTime.Singleton:
                    return new PerContainerLifetime();
                case LifeTime.Transient:
                default:
                    return null; // todo: determine if PerRequestLifeTime() should be used?
            }
        }

        private string ConvertKey(string key) => key ?? string.Empty;

        private string ConvertAddKey(string key, Type serviceType) => key
            ?? (ContainsService(serviceType, key) ? Guid.NewGuid().ToString() : ""); // hack: critical for AllInstances to be resolved correctly 
    }
}
