using DotNetStarter.Abstractions;
using DotNetStarter.Internal;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: LocatorRegistryFactory(typeof(DotNetStarter.StructureMapFactory))]

namespace DotNetStarter
{
    /// <summary>
    /// Locator with Structuremap Container 
    /// </summary>
    public class StructureMapFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates Structuremap Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new StructureMapLocator();
    }

    /// <summary>
    /// Structuremap Locator
    /// </summary>
    public class StructureMapLocator : ILocatorRegistry, ILocatorSetContainer
    {
        private IContainer _Container;

        /// <summary>
        /// Constructor
        /// </summary>
        public StructureMapLocator(IContainer container = null)
        {
            _Container = container ?? new Container();
        }

        /// <summary>
        /// Debug Information
        /// </summary>
        public string DebugInfo => _Container.WhatDoIHave();

        /// <summary>
        /// Raw structuremap container
        /// </summary>
        public object InternalContainer => _Container;

        /// <summary>
        /// Add object instance
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _Container.Configure(x => x.For(serviceType).Singleton().Use(serviceInstance));
        }

        /// <summary>
        /// Add by delegate
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Func<object> implementationFactory, LifeTime lifeTime)
        {
            _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use((context) => implementationFactory.Invoke()));
        }

        /// <summary>
        /// Add by type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifeTime"></param>
        /// <param name="constructorType"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, LifeTime lifeTime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest)
        {
            _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));
        }

        /// <summary>
        /// Add by generic
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        /// <param name="constructorType"></param>
        public void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime, ConstructorType.Empty);
        }

#if NET40 || NET45 || NETSTANDARD
        private ILifecycle ConvertLifeTime(LifeTime lifetime)
        {
            switch (lifetime)
            {
                case LifeTime.Transient:
                    return Lifecycles.Transient;
                case LifeTime.Singleton:
                    return Lifecycles.Singleton;
                case LifeTime.HttpRequest:
                case LifeTime.Scoped:
                    return Lifecycles.Container;
                case LifeTime.AlwaysUnique:
                    return Lifecycles.Unique;

            }

            return Lifecycles.Transient;
        }
#else
        private InstanceScope ConvertLifeTime(LifeTime lifetime)
        {
            switch (lifetime)
            {
                case LifeTime.Transient:
                    return InstanceScope.Transient;
                case LifeTime.Scoped: // assumption here that this is for a web app.
                case LifeTime.HttpRequest:
                    return InstanceScope.HttpContext;
                case LifeTime.Singleton:
                    return InstanceScope.Singleton;
                case LifeTime.AlwaysUnique:
                    return InstanceScope.Unique;
            }

            return InstanceScope.Transient;
        }
#endif

        /// <summary>
        /// Build up objects properties 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BuildUp(object target)
        {
            try
            {
                _Container.BuildUp(target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if service is registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null)
        {
            if (key == null)
                return _Container.TryGetInstance(serviceType) != null;

            return _Container.TryGetInstance(serviceType, key) != null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _Container?.Dispose();
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null)
        {
            try
            {
                return _Container.GetInstance(serviceType);
            }
            catch { return null; }
        }

        /// <summary>
        /// Get typed item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null)
        {
            try
            {
                return _Container.GetInstance<T>();
            }
            catch { return default(T); }
        }

        /// <summary>
        /// Get all registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Container.GetAllInstances(serviceType).OfType<object>();
        }

        /// <summary>
        /// Get all registered as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _Container.GetAllInstances<T>();
        }

        /// <summary>
        /// Remove service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            if (serviceImplementation == null)
            {
                _Container.Model.EjectAndRemove(serviceType);
            }
            else
            {
                _Container.Model.EjectAndRemoveTypes((type) =>
                {
                    if (type != serviceType)
                        return false;

                    return serviceImplementation.IsAssignableFromCheck(type);
                });
            }
        }

        /// <summary>
        /// Creates a scoped container
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
#if NET35
            throw new NotImplementedException();
#else
            return new StructureMapLocator(_Container.CreateChildContainer());
#endif
        }

        /// <summary>
        /// Allows container to be set externally, example is ConfigureServices in a netcore app
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(object container)
        {
            var tempContainer = container as IContainer;

            if(tempContainer == null)
                throw new ArgumentException($"{container} doesn't implement {typeof(IContainer).FullName}!");

            _Container = tempContainer;
        }
    }
}