namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using StructureMap;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base locator for StructureMap
    /// </summary>
    public abstract class StructureMapLocatorBase : ILocator, ILocatorCreateScope
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
        /// Creates a scoped container
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        public virtual ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
#if NET35
            return ThrowStructuremapNet35Exception();
#else
            return new StructureMapLocator(_Container.CreateChildContainer());
#endif
        }

        /// <summary>
        /// Creates/opens locator scope
        /// </summary>
        /// <param name="scopeKind"></param>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope(IScopeKind scopeKind)
        {
#if NET35
            return ThrowStructuremapNet35Exception() as ILocatorScoped;
#else
            return new StructureMapLocatorScoped(_Container.CreateChildContainer(), this, scopeKind);
#endif
        }

        private ILocator ThrowStructuremapNet35Exception()
        {
            throw new NotImplementedException("Structuremap in netframework v3.5 doesn't support scoping. DotNetStarter.DryIoc will work in these cases!");
        }
    }
}