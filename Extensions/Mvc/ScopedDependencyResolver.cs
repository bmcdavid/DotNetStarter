﻿using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Requires DotNetStarter.Web and Microsoft.AspNet.Mvc packages
    /// </summary>
    public class ScopedDependencyResolver : IDependencyResolver
    {
        static readonly Type _LocatorType = typeof(ILocator);
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
            var locator = ResolveLocator();

            if (serviceType == _LocatorType)
            {
                return _Locator; // use scoped locator if requested for injection
            }

            return locator.Get(serviceType);
        }

        /// <summary>
        /// Gets services from scoped locator if available
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ResolveLocator().GetAll(serviceType);
        }

        private ILocator ResolveLocator()
        {
            return HttpContext.Current?.GetScopedLocator() ?? _Locator;
        }
    }
}