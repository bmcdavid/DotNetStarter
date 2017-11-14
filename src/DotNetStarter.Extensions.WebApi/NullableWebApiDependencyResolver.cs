using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Nullable depencency resolver for Web Api.
    /// </summary>
    public class NullableWebApiDependencyResolver : NullableDependencyResolverBase, IDependencyResolver
    {
        private static readonly Type _LocatorType = typeof(ILocator);
        private readonly IHttpContextProvider _HttpContextProvider;
        private readonly ILocatorScopedFactory _LocatorScopeFactory;
        private readonly IPipelineScope _PipelineScope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="locatorScopedFactory"></param>
        /// <param name="httpContextProvider"></param>
        /// <param name="reflectionHelper"></param>
        /// <param name="pipelineScope"></param>
        /// <param name="allowableNullForGetService"></param>
        /// <param name="allowableNullForGetServices"></param>
        public NullableWebApiDependencyResolver(ILocator locator, ILocatorScopedFactory locatorScopedFactory, IHttpContextProvider httpContextProvider, IReflectionHelper reflectionHelper = null, IPipelineScope pipelineScope = null, IEnumerable<Type> allowableNullForGetService = null, IEnumerable<Type> allowableNullForGetServices = null) :
            base(locator, reflectionHelper, allowableNullForGetService, allowableNullForGetServices)
        {
            _HttpContextProvider = httpContextProvider;
            _LocatorScopeFactory = locatorScopedFactory;
            _PipelineScope = pipelineScope ?? _Locator.Get<IPipelineScope>();
        }

        /// <summary>
        /// Open a scoped locator
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            return new NullableWebApiDependencyResolver(_LocatorScopeFactory.CreateScope(), _LocatorScopeFactory, _HttpContextProvider, _ReflectionHelper, _PipelineScope, _AllowableNullForGetService, _AllowableNullForGetServices);
        }

        /// <summary>
        /// Dispose a scoped locator
        /// </summary>
        public void Dispose()
        {
            _Locator?.Dispose();
        }

        /// <summary>
        /// Default nullable services for Web Api GetService
        /// </summary>
        /// <returns></returns>
        public virtual object GetService(Type serviceType) => base.TryGetService(serviceType);

        /// <summary>
        /// Default nullable services for Web Api GetServices
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<object> GetServices(Type serviceType) => base.TryGetServices(serviceType);

        /// <summary>
        /// Default nullable services for Web Api GetService
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> DefaultAllowableNullForGetService()
        {
            return new Type[]
                {
                    typeof(System.Web.Http.Controllers.IActionValueBinder),
                    typeof(System.Web.Http.Controllers.IHttpActionSelector),
                    typeof(System.Web.Http.Controllers.IHttpActionInvoker),
                    typeof(System.Web.Http.Dispatcher.IHttpControllerActivator),
                    typeof(System.Web.Http.Dispatcher.IHttpControllerSelector),
                    typeof(System.Web.Http.Dispatcher.IAssembliesResolver),
                    typeof(System.Web.Http.Dispatcher.IHttpControllerTypeResolver),
                    typeof(System.Net.Http.Formatting.IContentNegotiator),
                    typeof(System.Web.Http.Hosting.IHostBufferPolicySelector),
                    typeof(System.Web.Http.Metadata.ModelMetadataProvider),
                    typeof(System.Web.Http.Tracing.ITraceManager),
                    typeof(System.Web.Http.Tracing.ITraceWriter),
                    typeof(System.Web.Http.Validation.IBodyModelValidator)
                };
        }

        /// <summary>
        /// Default nullable services for Web Api GetServices
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> DefaultAllowableNullForGetServices()
        {
            return new Type[] { };
        }

        /// <summary>
        /// Resolves locator from scope if possible, otherwise unscoped locator
        /// </summary>
        /// <returns></returns>
        protected override ILocator ResolveLocator()
        {
            return _PipelineScope.Enabled == true ? (_HttpContextProvider.CurrentContext?.GetScopedLocator() ?? _Locator) : _Locator;
        }
    }
}
