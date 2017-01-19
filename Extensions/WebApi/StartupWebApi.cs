using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Web.Http;

// adds controller implementations to the assembly scanner
[assembly: ScanTypeRegistry(typeof(ApiController))]

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Startup configuration for web api
    /// </summary>
    [StartupModule]
    public class StartupWebApi : IStartupModule
    {
        /// <summary>
        /// Shutdown
        /// </summary>
        /// <param name="engine"></param>
        public void Shutdown(IStartupEngine engine) { }

        /// <summary>
        /// Register API controllers
        /// </summary>
        /// <param name="engine"></param>
        public void Startup(IStartupEngine engine)
        {
            RegisterApiControllers(engine.Locator);

            GlobalConfiguration.Configure((config) => Register(config, engine.Locator));
        }

        static void RegisterApiControllers(ILocator locator)
        {
            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = registry.Get<IAssemblyScanner>()?.GetTypesFor(typeof(ApiController));

            foreach (var controller in controllerTypes)
            {
                registry?.Add(controller, controller, lifeTime: LifeTime.Scoped);
            }
        }

        static void Register(HttpConfiguration config, ILocator locator)
        {
            config.DependencyResolver = new WebApiDependencyResolver(locator);            
        }
    }
}
