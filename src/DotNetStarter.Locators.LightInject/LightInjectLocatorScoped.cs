using DotNetStarter.Abstractions;
using LightInject;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// LightInject ILocatorScoped
    /// </summary>
    public sealed class LightInjectLocatorScoped : ILocatorScoped, ILocatorCreateScope
    {
        private Action _disposeAction;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="locator"></param>
        public LightInjectLocatorScoped(Scope scope, ILocator locator)
        {
            Parent = locator as ILocatorScoped;
            _Scope = scope;
        }

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        private readonly Scope _Scope;

        /// <summary>
        /// Debug Info
        /// </summary>
        public string DebugInfo { get; } = "not supported";

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (!_Scope.IsDisposed)
            {
                _disposeAction?.Invoke();
                _Scope.Dispose();
                _Scope.IsDisposed = true;
            }
        }

        /// <summary>
        /// Gets a service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null)
        {
           return _Scope.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets a service type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null)
        {
            return _Scope.GetInstance<T>();
        }

        /// <summary>
        /// Gets all of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _Scope.GetInstance<IEnumerable<T>>();
        }

        /// <summary>
        /// Gets all of service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Scope.GetAllInstances(serviceType);
        }
        
        /// <summary>
        /// Creates a child scoped locator
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope()
        {
            return new LightInjectLocatorScoped(_Scope.BeginScope(), this);
        }

        /// <summary>
        /// Action to perform on disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction)
        {
            _disposeAction += disposeAction;
        }
    }
}
