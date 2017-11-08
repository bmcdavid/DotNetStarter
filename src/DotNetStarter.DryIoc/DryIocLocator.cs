namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using DryIoc;
    using System;
    using System.Linq;

    /// <summary>
    /// Creates a locator based on DryIoc.dll
    /// </summary>
    public class DryIocLocator : DryIocLocatorBase, ILocatorRegistry, ILocatorSetContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DryIocLocator(IContainer container = null) : base(container) { }

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
            RegisterSimple(_Container, serviceType, serviceImplementation, ConvertLifeTime(lifetime), constructorType, key);
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
            RegisterSimple<TService, TImpl>(_Container, ConvertLifeTime(lifetime), constructorType, key);
        }

        /// <summary>
        /// Addes service type to container given a factory to create the type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifeTime"></param>
        public virtual void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime)
        {
            _Container.RegisterDelegate(serviceType, r => implementationFactory(r.Resolve<ILocator>()), ConvertLifeTime(lifeTime));
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            _Container.RegisterDelegate(serviceType, resolver => serviceInstance, ConvertLifeTime(LifeTime.Singleton));
        }

        /// <summary>
        /// Determines if container has service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsService(Type serviceType, string key = null) => _Container.IsRegistered(serviceType, key);

        /// <summary>
        /// Removes a service, note: not all containers support this.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public virtual void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            if (serviceImplementation == null)
            {
                _Container.Unregister(serviceType, key, FactoryType.Service, (f) => true);
            }
            else
            {
                _Container.Unregister(serviceType, key, FactoryType.Service, (f) => f.ImplementationType == serviceType);
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

        private static IReuse ConvertLifeTime(LifeTime lifetime)
        {
            switch (lifetime)
            {
                //case LifeTime.Container: // in dryioc really not container scope, so just treat as a singleton in the container
                case LifeTime.Singleton:
                    return Reuse.Singleton;

                case LifeTime.Transient:
                    return Reuse.Transient;
                // scoping via the container isn't supported in the locator by default, it takes an unwrapped container to utilize this via cast of IContainer as IContainerRegistry
                case LifeTime.Scoped:
                    return Reuse.InCurrentScope;
                // web scoping takes special context which is setup in a special IContainerRegistry setup in the DotNetStarter.Web package
                case LifeTime.HttpRequest:
                    return Reuse.InWebRequest;
            }

            return Reuse.Transient;
        }

        private static Made GetConstructorFor(DryIoc.IContainer register, Type implementationType, ConstructorType constructor = ConstructorType.Greediest)
        {
            if (constructor == ConstructorType.Resolved)
            {
                return FactoryMethod.ConstructorWithResolvableArguments;
            }

            var allConstructors = implementationType.Constructors()
                                                    .Where(x => x.IsConstructor && x.IsPublic)
                                                    .OrderByDescending(x => x.GetParameters().Count());

            switch (constructor)
            {
                case ConstructorType.Empty:
                    return Made.Of(allConstructors.LastOrDefault());

                case ConstructorType.Greediest:
                default:
                    return Made.Of(allConstructors.FirstOrDefault());
            }
        }

        private static void RegisterSimple<TInterface, TImplementation>(DryIoc.IContainer register, IReuse reuse = null, ConstructorType constructor = ConstructorType.Greediest, string key = null)
            where TImplementation : TInterface
        {
            register.Register<TInterface, TImplementation>(reuse: reuse,
                made: GetConstructorFor(register, typeof(TImplementation), constructor),
                serviceKey: key);
        }

        private static void RegisterSimple(DryIoc.IContainer register, Type service, Type implementation, IReuse reuse = null, ConstructorType constructor = ConstructorType.Greediest, string key = null)
        {
            //note: evaluate how these can be better, example in netcore has issue
            //   Microsoft.AspNetCore.Server.Kestrel.Internal.KestrelServerOptionsSetup cannot be converted to Microsoft.Extensions.Options.IConfigureOptions`1[[Microsoft.AspNetCore.Server.Kestrel.KestrelServerOptions, Microsoft.AspNetCore.Server.Kestrel, Version=1.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60]]!

            if (!service.IsAssignableFromCheck(implementation))
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

            register.Register(service, implementation, reuse: reuse, made: GetConstructorFor(register, implementation, constructor), serviceKey: key);
        }

        private static void ThrowRegisterException(Type service, Type implementation)
        {
            var ex = new ArgumentException($"{implementation.FullName} cannot be converted to {service.FullName}!");

            throw ex;
        }
    }
}