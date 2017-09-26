namespace DotNetStarter.Extensions.Episerver
{
    using DotNetStarter.Abstractions;
    using StructureMap;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base Episerver Structuremap locator
    /// </summary>
    public abstract class EpiserverStructuremapLocatorBase : ILocator
    {
        /// <summary>
        /// Structuremap container
        /// </summary>
        protected IContainer _Container;

        /// <summary>
        /// Locator wrapping Episerver's structuremap container
        /// </summary>
        /// <param name="container"></param>
        public EpiserverStructuremapLocatorBase(IContainer container)
        {
            _Container = container;
        }

        /// <summary>
        /// Debug Information
        /// </summary>
        public string DebugInfo => _Container.WhatDoIHave();

        /// <summary>
        /// Container access
        /// </summary>
        public virtual object InternalContainer => _Container;

        /// <summary>
        /// Buildup object
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BuildUp(object target)
        {
            _Container.BuildUp(target);

            return true;
        }

        /// <summary>
        /// Cleanup container
        /// </summary>
        public void Dispose()
        {
            _Container?.Dispose();
        }

        /// <summary>
        /// Get service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type serviceType, string key = null)
        {
            return _Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Get service generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key = null)
        {
            return _Container.GetInstance<T>();
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Container.GetAllInstances(serviceType).OfType<object>();
        }

        /// <summary>
        /// Get all services generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _Container.GetAllInstances<T>();
        }

        /// <summary>
        /// Open container scope
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            return new EpiserverStructuremapLocator(_Container.CreateChildContainer());
        }
    }
}