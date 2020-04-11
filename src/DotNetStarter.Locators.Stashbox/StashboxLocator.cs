using DotNetStarter.Abstractions;
using Stashbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Stashbox Locator
    /// </summary>
    public class StashboxLocator : ILocator, ILocatorWithCreateScope, IServiceProvider
    {
        private readonly IStashboxContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public StashboxLocator(IStashboxContainer container) => _container = container;

        /// <summary>
        /// Creates a scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope() => new StashboxScopedLocator(_container.BeginScope(), this);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => _container.Dispose();

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
                return _container.ResolveAll(serviceType).Last();
            }

            return _container.Resolve(serviceType);
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
        public IEnumerable<T> GetAll<T>(string key = null) => _container.ResolveAll<T>();

        /// <summary>
        /// Gets all serviceType instances
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => _container.ResolveAll(serviceType);

        /// <summary>
        /// IServiceProvider.GetService
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType) => _container.Resolve(serviceType);
    }
}