namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using Lamar;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base locator for Lamar
    /// </summary>
    public class LamarLocator : ILocator, ILocatorWithCreateScope, ILocatorWithDebugInfo, IServiceProvider
    {
        /// <summary>
        /// Lamar container
        /// </summary>
        private readonly Container _container;

        /// <summary>
        /// Constructor
        /// </summary>
        public LamarLocator(Container container) => _container = container;

        /// <summary>
        /// Debug Information
        /// </summary>
        public string DebugInfo => _container.WhatDoIHave();

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() => _container.Dispose();

        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type serviceType, string key = null) => _container.GetInstance(serviceType);

        /// <summary>
        /// Get typed item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key = null) => _container.GetInstance<T>();

        /// <summary>
        /// Get all registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null) => _container.GetAllInstances(serviceType).OfType<object>();

        /// <summary>
        /// Get all registered as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll<T>(string key = null) => _container.GetAllInstances<T>();

        /// <summary>
        /// Creates/opens locator scope
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope() => new LamarLocatorScoped(_container.CreateScope(), this);

        /// <summary>
        /// IServiceProvider.GetService
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType) => _container.GetInstance(serviceType);
    }
}