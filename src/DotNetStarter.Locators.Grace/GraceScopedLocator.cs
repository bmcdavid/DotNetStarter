using DotNetStarter.Abstractions;
using Grace.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Scoped Grace Locator
    /// </summary>
    public class GraceScopedLocator : ILocatorScoped, ILocatorWithCreateScope
    {
        private readonly IExportLocatorScope _exportLocatorScope;
        private Action _onDispose;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exportLocatorScope"></param>
        /// <param name="locator"></param>
        public GraceScopedLocator(IExportLocatorScope exportLocatorScope, ILocator locator)
        {
            _exportLocatorScope = exportLocatorScope;
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
        public ILocatorScoped CreateScope() => new GraceScopedLocator(_exportLocatorScope.BeginLifetimeScope(), this);

        /// <summary>
        /// Dispose child container
        /// </summary>
        public void Dispose()
        {
            _onDispose?.Invoke();
            _exportLocatorScope.Dispose();
        }

        /// <summary>
        /// Gets a scoped instance of service Type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        public object Get(Type serviceType, string key = null) => _exportLocatorScope.Locate(serviceType);

        /// <summary>
        /// Gets a scoped instance T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => _exportLocatorScope.Locate<T>();

        /// <summary>
        /// Gets all scoped T instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null) => _exportLocatorScope.LocateAll<T>();

        /// <summary>
        /// Gets all scoped instances
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => _exportLocatorScope.LocateAll(serviceType);

        /// <summary>
        /// Action to perform on disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction) => _onDispose += disposeAction;
    }
}