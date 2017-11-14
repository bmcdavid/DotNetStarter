namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using StructureMap;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base locator for StructureMap
    /// </summary>
    public abstract class StructureMapLocatorBase : ILocator, ILocatorCreateScope, ILocatorWithPropertyInjection
    {
        /// <summary>
        /// StructureMap container
        /// </summary>
        protected IContainer _Container;

        /// <summary>
        /// Constructor
        /// </summary>
        public StructureMapLocatorBase(IContainer container = null)
        {
            _Container = container ?? new Container();
        }

        /// <summary>
        /// Debug Information
        /// </summary>
        public string DebugInfo => _Container.WhatDoIHave();

        /// <summary>
        /// Raw structuremap container
        /// </summary>
        public virtual object InternalContainer => _Container;

        /// <summary>
        /// Build up objects properties
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BuildUp(object target)
        {
            _Container.BuildUp(target);

            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _Container?.Dispose();
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type serviceType, string key = null)
        {
            return _Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Get typed item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key = null)
        {
            return _Container.GetInstance<T>();
        }

        /// <summary>
        /// Get all registered
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Container.GetAllInstances(serviceType).OfType<object>();
        }

        /// <summary>
        /// Get all registered as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll<T>(string key = null)
        {
            return _Container.GetAllInstances<T>();
        }

        /// <summary>
        /// Creates/opens locator scope
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope()
        {
            return new StructureMapLocatorScoped(_Container.CreateChildContainer(), this);
        }        
    }
}