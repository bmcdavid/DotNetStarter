namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using StructureMap;
    using StructureMap.Pipeline;
    using System;

    /// <summary>
    /// Structuremap Locator
    /// </summary>
    public class StructureMapLocatorRegistry : ILocatorRegistry, ILocatorRegistryWithContains, ILocatorRegistryWithRemove
    {
        /// <summary>
        /// StructureMap container
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        public StructureMapLocatorRegistry(IContainer container) => _container = container;

        /// <summary>
        /// Raw structuremap container
        /// </summary>
        public virtual object InternalContainer => _container;

        /// <summary>
        /// Add object instance
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance) => _container.Configure(x => x.For(serviceType).Singleton().Use(serviceInstance));

        /// <summary>
        /// Add by delegate
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifeTime) => _container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use((context) => implementationFactory.Invoke(context.GetInstance<ILocatorAmbient>().Current)));

        /// <summary>
        /// Add by type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifeTime"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifeTime = Lifecycle.Transient) => _container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));

        /// <summary>
        /// Add by generic
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        public void Add<TService, TImpl>(string key = null, Lifecycle lifetime = Lifecycle.Transient) where TImpl : TService => Add(typeof(TService), typeof(TImpl), key, lifetime);

        /// <summary>
        /// Checks if service is registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsService(Type serviceType, string key = null)
        {
            if (key is null)
            {
                return _container.TryGetInstance(serviceType) is object;
            }

            return _container.TryGetInstance(serviceType, key) is object;
        }

        /// <summary>
        /// Remove service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            if (serviceImplementation is null)
            {
                _container.Model.EjectAndRemove(serviceType);
            }
            else
            {
                _container.Model.EjectAndRemoveTypes((type) =>
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