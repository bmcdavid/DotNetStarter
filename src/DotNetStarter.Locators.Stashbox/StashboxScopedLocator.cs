using DotNetStarter.Abstractions;
using Stashbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Stashbox Scoped Locator
    /// </summary>
    public class StashboxScopedLocator : ILocatorScoped, ILocatorWithCreateScope, IServiceProvider
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
        public object Get(Type serviceType, string key = null)
        {
            if (typeof(IServiceProvider).IsAssignableFrom(serviceType))
            {
                // hack to fix, Resolve<IServiceProvider> returns its own
                return _dependencyResolver.ResolveAll(serviceType).Last();
            }

            return _dependencyResolver.Resolve(serviceType);
        }

        /// <summary>
        /// Gets service T instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => (T)Get(typeof(T), key);

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
        /// IServiceProvider.GetService
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType) => _dependencyResolver.Resolve(serviceType);

        /// <summary>
        /// Dispose action
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction) => _disposeAction += disposeAction;
    }
}