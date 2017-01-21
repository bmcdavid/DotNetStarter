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
        IApiControllerRegistrationSetup _ApiControllerRegistrationSetup;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiControllerRegistrationSetup"></param>
        public StartupWebApi(IApiControllerRegistrationSetup apiControllerRegistrationSetup)
        {
            _ApiControllerRegistrationSetup = apiControllerRegistrationSetup;
        }

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
            if (_ApiControllerRegistrationSetup?.EnableApiControllerRegistrations == true)
                RegisterApiControllers(engine.Locator, _ApiControllerRegistrationSetup.ApiControllerLifeTime);

            // throws exception  and don't want to call  GlobalConfiguration.Configuration.EnsureInitialized();
            // updating readme.txt to reflect proper wire-up.
            //GlobalConfiguration.Configure(Register);
        }

        static void RegisterApiControllers(ILocator locator, LifeTime lifetime)
        {
            if (locator == null)
                throw new ArgumentNullException($"{nameof(locator)} cannot be null, please install a locator package such as DotNetStarter.DryIoc or DotNetStart.Structuremap!");

            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = registry.Get<IAssemblyScanner>()?.GetTypesFor(typeof(ApiController));

            foreach (var controller in controllerTypes)
            {
                registry?.Add(controller, controller, lifeTime: lifetime);
            }
        }
    }
}
