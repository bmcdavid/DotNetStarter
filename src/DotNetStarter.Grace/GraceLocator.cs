[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.GraceLocatorFactory))]

namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using Grace.DependencyInjection;
    using Grace.DependencyInjection.Impl;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Creates a locator based on Grace
    /// </summary>
    public class GraceLocator : ILocatorRegistry, ILocatorSetContainer
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected InjectionScope _Container;

        /// <summary>
        /// Constructor
        /// </summary>
        public GraceLocator(InjectionScope container = null)
        {
            _Container = container ?? new DependencyInjectionContainer();
        }

        /// <summary>
        /// Provides debug information about the container
        /// </summary>
        public string DebugInfo => _Container?.WhatDoIHave();

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
            _Container.Configure(c => c.Export(serviceImplementation).As(serviceType).Lifestyle.Singleton());
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
            throw new NotImplementedException();
            //_Container.RegisterDelegate(serviceType, r => implementationFactory(r.Resolve<ILocator>()), ConvertLifeTime(lifeTime));
        }

        /// <summary>
        /// Adds service type via object instance, which is set to a singleton lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            throw new NotImplementedException();
            //_Container.Configure(x =>
            //{
                
            //    ExportInstance(() => serviceType).
            //    As(serviceType).
            //    Lifestyle.Singleton().
            //    ExternallyOwned();
            //});
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
                _Container.Inject(target);

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
            return _Container.TryLocate(serviceType, out object o, withKey: key);
        }

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
        public virtual object Get(Type service, string key = null) => _Container.LocateOrDefault(service, null);

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
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null) => _Container.LocateAll(serviceType) ?? Enumerable.Empty<object>();

        /// <summary>
        /// Gets all registered services for generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<TService> GetAll<TService>(string key = null) => GetAll(typeof(TService), key).OfType<TService>();

        /// <summary>
        /// Creates a scoped container
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            return new GraceLocator(_Container.CreateChildScope(scopeName: scopeName?.ToString()) as InjectionScope);
        }

        /// <summary>
        /// Removes a service, note: not all containers support this.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        public virtual void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            throw new NotSupportedException();
            //_Container.Configure(c =>
            //{
            //    c.ClearExports(export => export.StrategyType == serviceType);
            //});
        }

        /// <summary>
        /// Allows container to be set externally, example is ConfigureServices in a netcore app
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(object container)
        {
            var tempContainer = container as InjectionScope;
            _Container = tempContainer ?? throw new ArgumentException($"{container} doesn't implement {typeof(InjectionScope).FullName}!");
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
    public class GraceLocatorFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates DryIoc Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new GraceLocator();
    }
}