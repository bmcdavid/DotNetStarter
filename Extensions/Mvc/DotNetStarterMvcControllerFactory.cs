using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Creates MVC controllers using locator
    /// </summary>
    public class DotNetStarterMvcControllerFactory : DefaultControllerFactory
    {
        private readonly IStartupContext _StartupContext;
        private readonly ILocator _Locator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startupContext"></param>
        public DotNetStarterMvcControllerFactory(IStartupContext startupContext)
        {
            _StartupContext = startupContext;
            _Locator = startupContext.Locator;
        }

        /// <summary>
        /// Creates MVC controller
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
                throw new ArgumentNullException(nameof(requestContext));

            if (controllerName == null)
                throw new ArgumentNullException(nameof(controllerName));

            var controllerType = base.GetControllerType(requestContext, controllerName);

            if (controllerType == null)
                throw new NullReferenceException($"Controller type {controllerType.FullName} not found");

            var controller = ResolveLocator(requestContext).Get(controllerType) as IController;

            if (controller == null)
                throw new ApplicationException(string.Format("No controller with name '{0}' found in {1}", controllerName, _Locator.GetType().FullName));

            return controller;
        }

        /// <summary>
        /// Tries to resolve scoped locator from request context with fallback to startup locator
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        protected virtual ILocator ResolveLocator(RequestContext requestContext)
        {
            return requestContext.HttpContext?.GetScopedLocator() ?? _Locator;
        }
    }
}
