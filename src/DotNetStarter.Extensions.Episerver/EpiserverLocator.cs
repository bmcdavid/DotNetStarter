namespace DotNetStarter.Extensions.Episerver
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using StructureMap;
    using StructureMap.Pipeline;
    using System;

    /// <summary>
    /// Locator wrapping Episerver _Container
    /// </summary>
    public class EpiserverStructuremapLocator : EpiserverStructuremapLocatorBase, ILocatorRegistry
    {
        /// <summary>
        /// Locator wrapping Episerver's structuremap container
        /// </summary>
        /// <param name="container"></param>
        public EpiserverStructuremapLocator(IContainer container) : base(container) { }

        /// <summary>
        /// Add instance
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public virtual void Add(Type serviceType, object serviceInstance)
        {
            _Container.Configure(x => x.For(serviceType).Singleton().Use(serviceInstance));
        }

        /// <summary>
        /// Add by factory
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public virtual void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifeTime)
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
        public virtual void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifeTime = Lifecycle.Transient)
        {
            _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));
        }

        /// <summary>
        /// Add by generics
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        public virtual void Add<TService, TImpl>(string key = null, Lifecycle lifetime = Lifecycle.Transient) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime);
        }

        /// <summary>
        /// Contains service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsService(Type serviceType, string key = null)
        {
            if (key == null)
                return _Container.TryGetInstance(serviceType) != null;

            return _Container.TryGetInstance(serviceType, key) != null;
        }



        /// <summary>
        /// Remove a service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public virtual void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
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

        private ILifecycle ConvertLifeTime(Lifecycle lifetime)
        {
            switch (lifetime)
            {
                case Lifecycle.Transient:
                    return Lifecycles.Transient;
                case Lifecycle.Singleton:
                    return Lifecycles.Singleton;
                case Lifecycle.Scoped:
                    return Lifecycles.Container;
            }

            return Lifecycles.Transient;
        }
    }
}