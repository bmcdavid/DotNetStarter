using DotNetStarter.Abstractions;
using Grace.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Grace Locator
    /// </summary>
    public class GraceLocator : ILocator, ILocatorWithCreateScope
    {
        private DependencyInjectionContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public GraceLocator(DependencyInjectionContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates/opens locator scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope() => new GraceScopedLocator(_container.BeginLifetimeScope(), this);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => _container.Dispose();

        /// <summary>
        /// Gets service instance given type , key not used
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null) => _container.Locate(serviceType);

        /// <summary>
        /// Gets service instance given type for generics
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => _container.Locate<T>();

        /// <summary>
        /// Gets all registered services for generics
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null) => _container.LocateAll<T>();

        /// <summary>
        /// Gets all registered services.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => _container.LocateAll(serviceType);
    }
}