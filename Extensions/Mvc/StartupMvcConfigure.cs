using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// adds controller implementations to the assembly scanner
[assembly: ScanTypeRegistry(typeof(IController))]

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Configure MVC
    /// </summary>
    [StartupModule(typeof(RegisterConfiguration))]
    public class StartupMvcConfigure : ILocatorConfigure
    {
        /// <summary>
        /// Allows for registration setup to be swapped before configuring controllers
        /// </summary>
        public static Func<IControllerRegistrationSetup> GetControllerRegistrationSetup;

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            registry.Add<IControllerFactory, DotNetStarterMvcControllerFactory>();

            engine.OnLocatorStartupComplete += () =>
            {
                IControllerRegistrationSetup controllerRegistrationSetup = GetControllerRegistrationSetup?.Invoke() ??
                    new ControllerRegistrationSetup();

                if (controllerRegistrationSetup.EnableControllerRegisterations == true)
                {
                    IEnumerable<Type> controllerTypes = engine.Configuration.AssemblyScanner.GetTypesFor(typeof(IController));

                    foreach (var controller in controllerTypes.Where(x => !x.IsAbstract && !x.IsInterface))
                    {
                        registry?.Add(controller, controller, lifeTime: controllerRegistrationSetup.ControllerLifeTime);
                    }
                }
            };
        }
    }
}
