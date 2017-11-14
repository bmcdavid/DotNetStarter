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

                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return null;
                }

                return Activator.CreateInstance(serviceType);
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
            return _HttpContextProvider.CurrentContext?.GetScopedLocator() ?? throw new Exception("Unable to get scoped locator!");
        }
    }
}