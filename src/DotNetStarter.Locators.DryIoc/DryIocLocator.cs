namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DryIoc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// DryIoc locator
    /// </summary>
    public class DryIocLocator : ILocator, ILocatorWithCreateScope, ILocatorWithPropertyInjection, ILocatorWithDebugInfo, IServiceProvider
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected IContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        public DryIocLocator(IContainer container) => _container = container;

        /// <summary>
        /// Provides debug information about the container
        /// </summary>
        public string DebugInfo => _container?.GetServiceRegistrations()?.Select(x => x.ToString()).
            Aggregate((current, next) => current + Environment.NewLine + next);

        /// <summary>
        /// Builds up properties of given object, useful in webforms.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool BuildUp(object target) => _container.InjectPropertiesAndFields(target) is object;

        /// <summary>
        /// Creates/opens locator scope
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope() => new DryIocLocatorScoped(_container.OpenScope(), this);

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() => _container.Dispose();

        /// <summary>
        /// Gets service instance given type and optional key
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type service, string key = null) => _container.Resolve(service, key, IfUnresolved.Throw);

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
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null) => _container.ResolveMany(serviceType, serviceKey: key);

        /// <summary>
        /// Gets all registered services for generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<TService> GetAll<TService>(string key = null) => _container.ResolveMany<TService>(serviceKey: key);

        /// <summary>
        /// IServiceProvider.GetService
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType) => _container.Resolve(serviceType);
    }
}