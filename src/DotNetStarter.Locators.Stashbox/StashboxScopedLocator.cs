using DotNetStarter.Abstractions;
using Stashbox;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Stashbox Scoped Locator
    /// </summary>
    public class StashboxScopedLocator : ILocatorScoped, ILocatorWithCreateScope
    {
        private IDependencyResolver _dependencyResolver;
        private Action _disposeAction;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dependencyResolver"></param>
        /// <param name="stashboxLocator"></param>
        public StashboxScopedLocator(IDependencyResolver dependencyResolver, ILocator stashboxLocator)
        {
            _dependencyResolver = dependencyResolver;
            Parent = stashboxLocator as ILocatorScoped;
        }

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// Creates a child scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope() => new StashboxScopedLocator(_dependencyResolver.BeginScope(), this);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _disposeAction?.Invoke();
            _dependencyResolver.Dispose();
        }

        /// <summary>
        /// Gets serviceType instance
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null) => _dependencyResolver.Resolve(serviceType);

        /// <summary>
        /// Gets service T instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => _dependencyResolver.Resolve<T>();

        /// <summary>
        /// Gets all service T instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null) => _dependencyResolver.ResolveAll<T>();

        /// <summary>
        /// Gets all serviceType instances
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => _dependencyResolver.ResolveAll(serviceType);

        /// <summary>
        /// Dispose action
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction) => _disposeAction += disposeAction;
    }
}