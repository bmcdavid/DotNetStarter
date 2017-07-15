[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.DryIocLocatorFactory))]

namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using DryIoc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Creates a locator based on DryIoc.dll
    /// </summary>
    public class DryIocLocator : ILocatorRegistry, ILocatorSetContainer
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected IContainer _Container;

        /// <summary>
        /// Constructor
        /// </summary>
        public DryIocLocator(IContainer container = null)
        {
            var rules = Rules.Default
                .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                .WithTrackingDisposableTransients() //used in transient delegate cases
                .WithImplicitRootOpenScope()
                ;

            _Container = container ?? new Container(rules);
        }

        /// <summary>
        /// Provides debug information about the container
        /// </summary>
        public string DebugInfo => _Container?.GetServiceRegistrations()?.Select(x => x.ToString()).
            Aggregate((current, next) => current + Environment.NewLine + next);

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
        /// Builds up properties of given object, useful in webforms.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool BuildUp(object target)
        {
            try
            {
                _Container.InjectPropertiesAndFields(target); // v2.x

                return true;
            }
            catch (Exception e)
            {
                if (e is ContainerException ce)
                    throw new StartupContainerException(ce.Error, ce.Message, ce.InnerException);

                throw new StartupContainerException(-100, e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Determines if container has service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsService(Type serviceType, string key = null) => _Container.IsRegistered(serviceType, key);

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            _Container?.Dispose();
        }

        /// <summary>
        /// Gets service instance given type and optional key
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type service, string key = null) => _Container.Resolve(service, key, IfUnresolved.ReturnDefault);

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
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null) =>
                    _Container.ResolveMany(serviceType, serviceKey: key); //_Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType), IfUnresolved.ReturnDefault) as IEnumerable<object>;

        /// <summary>
        /// Gets all registered services for generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<TService> GetAll<TService>(string key = null) =>
                    _Container.ResolveMany<TService>(serviceKey: key); //_Container.Resolve<IEnumerable<TService>>(IfUnresolved.ReturnDefault);

        /// <summary>
        /// Creates a scoped container
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            var typedContext = scopeContext as IScopeContext;
            var container = _Container;

            if (typedContext != null)
                container = container.With(scopeContext: typedContext);

            return new DryIocLocator(container.OpenScope(scopeName));
        }

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
                //case LifeTime.Thread:
                //    return Reuse.InThread;
                case LifeTime.AlwaysUnique:
                    return Reuse.Transient;
            }

            return Reuse.Transient;
        }

        private static Made GetConstructorFor(DryIoc.IContainer register, Type implementationType, ConstructorType constructor = ConstructorType.Empty)
        {
            if (constructor == ConstructorType.Resolved)
            {
                return FactoryMethod.ConstructorWithResolvableArguments;
            }

            var allConstructors = implementationType.Constructors().Where(x => !x.IsStatic && !x.IsPrivate).OrderBy(x => x.GetParameters().Count());

            if (constructor == ConstructorType.Greediest)
                allConstructors.Reverse();

            // return the delegate Made.of for v2.x
            return Made.Of(allConstructors.FirstOrDefault());
        }

        private static void RegisterSimple<TInterface, TImplementation>(DryIoc.IContainer register, IReuse reuse = null, ConstructorType constructor = ConstructorType.Empty, string key = null)
            where TImplementation : TInterface
        {
            // for v2.x
            register.Register<TInterface, TImplementation>(reuse: reuse, made: GetConstructorFor(register, typeof(TImplementation), constructor), serviceKey: key);
        }

        private static void RegisterSimple(DryIoc.IContainer register, Type service, Type implementation, IReuse reuse = null, ConstructorType constructor = ConstructorType.Empty, string key = null)
        {
            //todo: evaluate how these can be better, example in netcore has issue
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

    /// <summary>
    /// Locator with DryIoc Container 
    /// </summary>
    public class DryIocLocatorFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates DryIoc Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new DryIocLocator();
    }
}