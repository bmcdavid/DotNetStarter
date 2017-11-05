namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using StructureMap;
    using StructureMap.Pipeline;
    using System;
    using System.Linq;

    // todo v2: remove net35 and net40 support to consolidate on single package version

    /// <summary>
    /// Structuremap Locator
    /// </summary>
    public class StructureMapLocator : StructureMapLocatorBase, ILocatorRegistry, ILocatorSetContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StructureMapLocator(IContainer container = null) : base(container) { }

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
#if NET35
                System.Diagnostics.Debug.WriteLine($"{typeof(StructureMapLocator).FullName} only supports greediest constructor types, if support is needed, please create a custom locator implementation!");
                _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));
#else
                var empty = serviceImplementation.Constructors().FirstOrDefault(x => x.GetParameters().Length == 0);
                _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation).Constructor = empty);
#endif
            }
            else
            {
                _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));
            }
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
            Add(typeof(TService), typeof(TImpl), key, lifetime, constructorType);
        }

#if NET40 || NET45 || NETSTANDARD1_3
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
        /// Allows container to be set externally, example is ConfigureServices in a netcore app
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(object container)
        {
            var tempContainer = container as IContainer;
            _Container = tempContainer ?? throw new ArgumentException($"{container} doesn't implement {typeof(IContainer).FullName}!");
        }
    }
}