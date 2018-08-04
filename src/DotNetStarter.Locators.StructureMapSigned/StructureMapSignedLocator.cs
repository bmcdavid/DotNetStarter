namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using StructureMap;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ILocator for StructureMap
    /// </summary>
    public class StructureMapSignedLocator : ILocator, ILocatorWithCreateScope, ILocatorWithPropertyInjection, ILocatorWithDebugInfo
    {
        /// <summary>
        /// StructureMap container
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        public StructureMapSignedLocator(IContainer container) => _container = container;

        /// <summary>
        /// Debug Information
        /// </summary>
        public string DebugInfo => _container.WhatDoIHave();

        /// <summary>
        /// Build up objects properties
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BuildUp(object target)
        {
            _container.BuildUp(target);

            return true;
        }

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
        public virtual ILocatorScoped CreateScope() => new StructureMapSignedLocatorScoped(_container.CreateChildContainer(), this);
    }
}