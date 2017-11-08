using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Requires DotNetStarter.Web and Microsoft.AspNet.Mvc packages
    /// </summary>
    public class ScopedDependencyResolver : IDependencyResolver
    {
        IHttpContextProvider _HttpContextProvider;
        ILocator _Locator;
        IServiceProviderTypeChecker _ServiceProviderTypeChecker;

        //todo: v2 remove obsolete constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        [Obsolete("Please use other constructor")]
        public ScopedDependencyResolver(ILocator locator) : this(locator, null, null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="serviceProviderTypeChecker"></param>
        /// <param name="httpContextProvider"></param>
        public ScopedDependencyResolver(ILocator locator, IServiceProviderTypeChecker serviceProviderTypeChecker, IHttpContextProvider httpContextProvider)
        {
            _Locator = locator;
            _ServiceProviderTypeChecker = serviceProviderTypeChecker ?? locator.Get<IServiceProviderTypeChecker>();
            _HttpContextProvider = httpContextProvider ?? locator.Get<IHttpContextProvider>();
        }

        /// <summary>
        /// Gets service from scoped locator if available
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType)
        {
            try
            {
                return ResolveLocator().Get(serviceType);
            }
            catch (Exception e)
            {
                if (_ServiceProviderTypeChecker.IsScannedAssembly(serviceType, e))
                {
                    throw;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets services from scoped locator if available
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return ResolveLocator().GetAll(serviceType);
            }
            catch(Exception e)
            {
                if (_ServiceProviderTypeChecker.IsScannedAssembly(serviceType, e))
                {
                    throw;
                }

                return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Tries to get the scoped locator from current HttpContext
        /// </summary>
        /// <returns></returns>
        protected virtual ILocator ResolveLocator()
        {
            // todo: v2, throw exception if GetScopedLocator() is null instead of using _Locator, since it messes up scoped registrations
            return _HttpContextProvider.CurrentContext?.GetScopedLocator() ?? _Locator;
        }
    }
}