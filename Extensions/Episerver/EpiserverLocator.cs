using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Locator wrapping Episerver _Container
    /// </summary>
    public class EpiserverStructuremapLocator : ILocatorRegistry
    {
        IContainer _Container;

        /// <summary>
        /// Locator wrapping Episerver's structuremap container
        /// </summary>
        /// <param name="container"></param>
        public EpiserverStructuremapLocator(IContainer container)
        {
            _Container = container;
        }

        /// <summary>
        /// Debug Information
        /// </summary>
        public string DebugInfo => _Container.WhatDoIHave();

        /// <summary>
        /// Container access
        /// </summary>
        public object InternalContainer => _Container;

        /// <summary>
        /// Add instance
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _Container.Configure(x => x.For(serviceType).Singleton().Use(serviceInstance));
        }

        /// <summary>
        /// Add by factory
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime)
        {
            _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use((context) => implementationFactory.Invoke(context.GetInstance<ILocator>())));
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
            if (constructorType == ConstructorType.Empty)
            {
                var empty = serviceImplementation.GetConstructor(Type.EmptyTypes);
                _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation).Constructor = empty);
            }
            else
            {
                _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));
            }
        }

        /// <summary>
        /// Add by generics
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        /// <param name="constructorType"></param>
        public void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime, constructorType);
        }

        /// <summary>
        /// Buildup object
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BuildUp(object target)
        {
            _Container.BuildUp(target);

            return true;
        }

        /// <summary>
        /// Contains service
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
        /// Cleanup container
        /// </summary>
        public void Dispose()
        {
            _Container?.Dispose();
        }

        /// <summary>
        /// Get service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null)
        {
            return _Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Get service generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null)
        {
            return _Container.GetInstance<T>();
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Container.GetAllInstances(serviceType).OfType<object>();
        }

        /// <summary>
        /// Get all services generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _Container.GetAllInstances<T>();
        }

        /// <summary>
        /// Open container scope
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            return new EpiserverStructuremapLocator(_Container.CreateChildContainer());
        }

        /// <summary>
        /// Remove a service
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
    }
}