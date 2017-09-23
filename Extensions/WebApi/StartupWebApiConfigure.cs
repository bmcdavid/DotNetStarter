﻿using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

// adds controller implementations to the assembly scanner
[assembly: ScanTypeRegistry(typeof(ApiController))]

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Registers ApiControllers to the locator
    /// </summary>
    public class StartupWebApiConfigure : ILocatorConfigure
    {
        /// <summary>
        /// Allows for registration setup to be swapped before configuring controllers
        /// </summary>
        public static Func<IApiControllerRegistrationSetup> GetApiControllerRegistrationSetup;

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            engine.OnLocatorStartupComplete += () =>
            {
                IApiControllerRegistrationSetup controllerRegistrationSetup = GetApiControllerRegistrationSetup?.Invoke() ??
                    new ApiControllerRegistrationSetup();

                if (controllerRegistrationSetup.EnableApiControllerRegistrations == true)
                {
                    IEnumerable<Type> controllerTypes = engine.Configuration.AssemblyScanner.GetTypesFor(typeof(ApiController));

                    foreach (var controller in controllerTypes.Where(x => x.IsAbstract == false))
                    {
                        registry?.Add(controller, controller, lifeTime: controllerRegistrationSetup.ApiControllerLifeTime);
                    }
                }
            };
        }
    }
}
