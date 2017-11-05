using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc
{
    // todo: remove obsolete, and change try catch to only return null if type isn't in a scanned assembly

    /// <summary>
    /// Requires DotNetStarter.Web and Microsoft.AspNet.Mvc packages
    /// </summary>
    [Obsolete("Please use NullableMvcDependencyResolver instead!")]
    public class ScopedDependencyResolver : IDependencyResolver
    {
        ILocator _Locator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        public ScopedDependencyResolver(ILocator locator)
        {
            _Locator = locator;
        }

        /// <summary>
        /// Gets service from scoped locator if available
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
        /// Gets services from scoped locator if available
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
            return HttpContext.Current?.GetScopedLocator() ?? _Locator;
        }
    }
}