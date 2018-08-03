using Autofac;
using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Scoped Autofac Locator
    /// </summary>
    public class AutofacScopedLocator : ILocatorScoped, ILocatorWithCreateScope
    {
        private ILifetimeScope _lifetimeScope;
        private Action _action;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="locator"></param>
        public AutofacScopedLocator(ILifetimeScope lifetimeScope, ILocator locator)
        {
            _lifetimeScope = lifetimeScope;
            Parent = locator as ILocatorScoped;
        }

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// Creates a child scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope() => new AutofacScopedLocator(_lifetimeScope.BeginLifetimeScope(), this);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _action?.Invoke();
            _lifetimeScope.Dispose();
        }

        /// <summary>
        /// Gets a service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null) => _lifetimeScope.Resolve(serviceType);

        /// <summary>
        /// Gets a service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => _lifetimeScope.Resolve<T>();

        /// <summary>
        /// Gets all services
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null) => _lifetimeScope.Resolve<IEnumerable<T>>();

        /// <summary>
        /// Gets all services
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => _lifetimeScope.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;

        void ILocatorScoped.OnDispose(Action disposeAction) => _action += disposeAction;
    }
}