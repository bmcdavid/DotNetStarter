using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Dependency Resolver for WebApi
    /// </summary>
    public class WebApiDependencyResolver : IDependencyResolver
    {
        ILocator _Locator;
        IPipelineScope _PipelineScope;
        private readonly IServiceProviderTypeChecker _ServiceProviderTypeChecker;
        private readonly IHttpContextProvider _HttpContextProvider;
        private readonly ILocatorScopedFactory _LocatorScopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="pipelineScope">Optional: if null, retrieved from locator</param>
        public WebApiDependencyResolver(ILocator locator, IPipelineScope pipelineScope) : this(locator, pipelineScope, serviceProviderTypeChecker: null)
        {
            _Locator = locator;
            _PipelineScope = pipelineScope ?? _Locator.Get<IPipelineScope>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="pipelineScope">Optional: if null, retrieved from locator</param>
        /// <param name="serviceProviderTypeChecker"></param>
        /// <param name="httpContextProvider"></param>
        /// <param name="locatorScopeFactory"></param>
        public WebApiDependencyResolver(ILocator locator, IPipelineScope pipelineScope = null, IServiceProviderTypeChecker serviceProviderTypeChecker = null, IHttpContextProvider httpContextProvider = null, ILocatorScopedFactory locatorScopeFactory = null)
        {
            _Locator = locator;
            _PipelineScope = pipelineScope ?? _Locator.Get<IPipelineScope>();
            _ServiceProviderTypeChecker = serviceProviderTypeChecker ?? locator.Get<IServiceProviderTypeChecker>();
            _HttpContextProvider = httpContextProvider ?? locator.Get<IHttpContextProvider>();
            _LocatorScopeFactory = locatorScopeFactory ?? locator.Get<ILocatorScopedFactory>();
        }

        /// <summary>
        /// Open a scoped locator
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            return new WebApiDependencyResolver(_LocatorScopeFactory.CreateScope(), _PipelineScope, _ServiceProviderTypeChecker, _HttpContextProvider);
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
            catch (Exception e)
            {
                if (_ServiceProviderTypeChecker.IsScannedAssembly(serviceType, e))
                {
                    throw;
                }

                return Enumerable.Empty<object>();
            }
        }
        
        private ILocator ResolveLocator()
        {
            return _PipelineScope.Enabled == true ? (_HttpContextProvider.CurrentContext?.GetScopedLocator() ?? _Locator) : _Locator;
        }
    }
}