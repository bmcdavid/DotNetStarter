using DotNetStarter.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Scoped Lamar locator
    /// </summary>
    public sealed class LamarLocatorScoped : ILocatorScoped, ILocatorWithCreateScope
    {
        private readonly IServiceScope _scope;
        private Action _onDispose;
        private ILocatorScoped _parent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="locator"></param>
        public LamarLocatorScoped(IServiceScope scope, ILocator locator)
        {
            _scope = scope;
            _parent = locator as ILocatorScoped;
        }

        ILocatorScoped ILocatorScoped.Parent => _parent;

        ILocatorScoped ILocatorWithCreateScope.CreateScope() => new LamarLocatorScoped(_scope.ServiceProvider.CreateScope(), this);

        void IDisposable.Dispose()
        {
            _onDispose?.Invoke();
            _scope.Dispose();
        }

        object ILocator.Get(Type serviceType, string key) => _scope.ServiceProvider.GetService(serviceType);

        T ILocator.Get<T>(string key) => (T)_scope.ServiceProvider.GetService(typeof(T));

        IEnumerable<T> ILocator.GetAll<T>(string key) => _scope.ServiceProvider.GetServices<T>();

        IEnumerable<object> ILocator.GetAll(Type serviceType, string key) => _scope.ServiceProvider.GetServices(serviceType);

        void ILocatorScoped.OnDispose(Action disposeAction) => _onDispose += disposeAction;
    }
}