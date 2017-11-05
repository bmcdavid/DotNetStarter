using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace DotNetStarter.Extensions.WebApi
{
    // todo: remove obsolete, and change try catch to only return null if type isn't in a scanned assembly

    /// <summary>
    /// Dependency Resolver for WebApi
    /// </summary>
    [Obsolete("Please use NullableWebApiDependencyResolver instead!")]
    public class WebApiDependencyResolver : IDependencyResolver
    {
        static readonly Type _LocatorType = typeof(ILocator);
        ILocator _Locator;
        IPipelineScope _PipelineScope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="pipelineScope">Optional: if null, retrieved from locator</param>
        public WebApiDependencyResolver(ILocator locator, IPipelineScope pipelineScope = null)
        {
            _Locator = locator;
            _PipelineScope = pipelineScope ?? _Locator.Get<IPipelineScope>();
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
            try
            {
                return ResolveLocator().Get(serviceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a scoped services from either http context locator or current scoped locator
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return ResolveLocator().GetAll(serviceType);
            }
            catch
            {
                return Enumerable.Empty<object>();
            }
        }

        private ILocator ResolveLocator()
        {
            return _PipelineScope.Enabled == true ? (HttpContext.Current?.GetScopedLocator() ?? _Locator) : _Locator;
        }
    }
}