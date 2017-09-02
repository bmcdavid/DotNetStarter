using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Nullable depencency resolver for MVC.
    /// </summary>
    public class NullableMvcDependencyResolver : NullableDependencyResolverBase, IDependencyResolver
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="reflection"></param>
        /// <param name="allowableNullForGetService"></param>
        /// <param name="allowableNullForGetServices"></param>
        public NullableMvcDependencyResolver(
            ILocator locator,
            IReflectionHelper reflection = null,
            IEnumerable<Type> allowableNullForGetService = null,
            IEnumerable<Type> allowableNullForGetServices = null) :
            base(locator, reflection, allowableNullForGetService, allowableNullForGetServices)
        { }

        /// <summary>
        /// Tries to get a service instance for given type from ILocator, if locator cannot find it, an exception is thrown unless defined as nullable
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType) => base.TryGetService(serviceType);

        /// <summary>
        /// Tries to get a service instance for given type from ILocator, if locator cannot find it, an exception is thrown unless defined as nullable
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetServices(Type serviceType) => base.TryGetServices(serviceType);

        /// <summary>
        /// Default nullable services for MVC GetService
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> DefaultAllowableNullForGetService()
        {
            // these types are in MVC 5
            List<string> unavailableTypesInMvc4 = new List<string>
            {
                "System.Web.Mvc.ITempDataProviderFactory",
                "System.Web.Mvc.IActionInvokerFactory",
                "System.Web.Mvc.Async.IAsyncActionInvokerFactory"
            };

            var types = new List<Type>
                {
                    typeof(IActionInvoker),
                    typeof(IAsyncActionInvoker),
                    typeof(IControllerActivator),
                    typeof(IControllerFactory),
                    typeof(ITempDataProvider),
                    typeof(IViewPageActivator),
                    typeof(WebViewPage)
                };

            foreach (string type in unavailableTypesInMvc4)
            {
                Type t = System.Web.Compilation.BuildManager.GetType(type, false, false);

                if (t != null)
                {
                    types.Add(t);
                }
            }

            return types;
        }

        /// <summary>
        /// Default nullable services for MVC GetServices
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> DefaultAllowableNullForGetServices()
        {
            return new Type[]
                {
                    typeof(IFilterProvider),
                    typeof(IViewEngine)
                };
        }

        /// <summary>
        /// Resolves locator from scope if possible, otherwise unscoped locator
        /// </summary>
        /// <returns></returns>
        protected override ILocator ResolveLocator()
        {
            return HttpContext.Current?.GetScopedLocator() ?? _Locator;
        }
    }
}
