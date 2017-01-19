using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Dependencies;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Dependency Resolver for WebApi
    /// </summary>
    public class WebApiDependencyResolver : IDependencyResolver
    {
        ILocator _Locator;

        Import<IPipelineScope> PipelineScope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        public WebApiDependencyResolver(ILocator locator)
        {
            _Locator = locator;
        }

        /// <summary>
        /// Open a scoped locator
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            return new WebApiDependencyResolver(_Locator.OpenScope());
        }

        /// <summary>
        /// Dispose a scoped locator
        /// </summary>
        public void Dispose()
        {
            _Locator?.Dispose();
        }

        /// <summary>
        /// Gets a scoped service from either http context locator or current scoped locator
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (PipelineScope.Service?.Enabled == true)
                return (HttpContext.Current?.GetScopedLocator() ?? _Locator).Get(serviceType);

            return _Locator.Get(serviceType);
        }

        /// <summary>
        /// Gets a scoped services from either http context locator or current scoped locator
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (PipelineScope.Service?.Enabled == true)
                return (HttpContext.Current?.GetScopedLocator() ?? _Locator).GetAll(serviceType);

            return _Locator.GetAll(serviceType);
        }
    }
}