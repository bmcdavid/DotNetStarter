namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using DryIoc;
    using System;
    using System.Linq;

    /// <summary>
    /// Creates a locator based on DryIoc.dll
    /// </summary>
    public class DryIocLocatorRegistry : ILocatorRegistry, ILocatorRegistryWithContains, ILocatorRegistryWithRemove
    {
        private readonly IContainer _container;
        private readonly bool _withResolvedArguments;

        /// <summary>
        /// Constructor
        /// </summary>
        public DryIocLocatorRegistry(IContainer container)
        {
            _container = container;
            _withResolvedArguments = container?.Rules?.FactoryMethod == FactoryMethod.ConstructorWithResolvableArguments;
        }

        /// <summary>
        /// Allows access to wrapped container
        /// </summary>
        public virtual object InternalContainer => _container;

        /// <summary>
        /// Adds service type to container, given its implementation type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        public virtual void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifetime = Lifecycle.Transient)
        {
            RegisterSimple(_container, serviceType, serviceImplementation, _withResolvedArguments, ConvertLifeTime(lifetime), key);
        }

        /// <summary>
        /// Adds service type to container, given its implementation type via generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifetime"></param>
        public virtual void Add<TService, TImpl>(string key = null, Lifecycle lifetime = Lifecycle.Transient)
            where TImpl : TService
        {
            RegisterSimple<TService, TImpl>(_container, _withResolvedArguments, ConvertLifeTime(lifetime), key);
        }

        /// <summary>
        /// Adds service type to container given a factory to create the type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public virtual void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifeTime)
        {
            _container.RegisterDelegate(serviceType, r => implementationFactory(r.Resolve<ILocatorAmbient>().Current), ConvertLifeTime(lifeTime));
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _container.RegisterDelegate(serviceType, resolver => serviceInstance, ConvertLifeTime(Lifecycle.Singleton));
        }

        /// <summary>
        /// Determines if container has service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsService(Type serviceType, string key = null) => _container.IsRegistered(serviceType, key);

        /// <summary>
        /// Removes a service, note: not all containers support this.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public virtual void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            if (serviceImplementation is null)
            {
                _container.Unregister(serviceType, key, FactoryType.Service, (f) => true);
            }
            else
            {
                _container.Unregister(serviceType, key, FactoryType.Service, (f) => f.ImplementationType == serviceType);
            }
        }

        private static IReuse ConvertLifeTime(Lifecycle lifetime)
        {
            switch (lifetime)
            {
                case Lifecycle.Singleton:
                    return Reuse.Singleton;

                case Lifecycle.Transient:
                    return Reuse.Transient;
                // scoping via the container isn't supported in the locator by default, it takes an unwrapped container to utilize this via cast of IContainer as IContainerRegistry
                case Lifecycle.Scoped:
                    return Reuse.ScopedOrSingleton;
            }

            return Reuse.Transient;
        }

        private static Made GetConstructorFor(IContainer register, Type implementationType)
        {
            var allConstructors = implementationType.Constructors()
                .Where(x => x.IsConstructor && x.IsPublic)
                .OrderByDescending(x => x.GetParameters().Count());

            return Made.Of(allConstructors.FirstOrDefault());
        }

        private static void RegisterSimple<TInterface, TImplementation>(IContainer register, bool withResolvedArguments, IReuse reuse = null, string key = null)
            where TImplementation : TInterface
        {
            register.Register<TInterface, TImplementation>(reuse: reuse,
                made: withResolvedArguments ? null : GetConstructorFor(register, typeof(TImplementation)),
                serviceKey: key);
        }

        private static void RegisterSimple(IContainer register, Type service, Type implementation, bool withResolvedArguments, IReuse reuse = null, string key = null)
        {
            RegistryExtensions.ConfirmService(service, implementation);

            register.Register(service, implementation, reuse: reuse, made: withResolvedArguments ? null : GetConstructorFor(register, implementation), serviceKey: key);
        }
    }
}