using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

// adds controller implementations to the assembly scanner
[assembly: ScanTypeRegistry(typeof(IController))]

namespace DotNetStarter.Mvc
{
    /// <summary>
    /// Requires DotNetStarter.Web and Microsoft.AspNet.Mvc packages
    /// </summary>
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
            return (HttpContext.Current?.GetScopedLocator() ?? _Locator).Get(serviceType);
        }

        /// <summary>
        /// Gets services from scoped locator if available
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (HttpContext.Current?.GetScopedLocator() ?? _Locator).GetAll(serviceType);
        }
    }
}