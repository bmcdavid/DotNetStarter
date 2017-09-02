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
    /// executes on DotNetStarter startup to set MVC dependency resolver and register controllers
    /// </summary>
    [StartupModule]
    public class StartupMvc : IStartupModule
    {
        IControllerRegistrationSetup _ControllerRegistrationSetup;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllerRegistrationSetup"></param>
        public StartupMvc(IControllerRegistrationSetup controllerRegistrationSetup)
        {            
            _ControllerRegistrationSetup = controllerRegistrationSetup;
        }

        /// <summary>
        /// shutdown
        /// </summary>
        /// <param name="engine"></param>
        public void Shutdown(IStartupEngine engine) { }

        /// <summary>
        /// Register controllers and set dependency resolver
        /// </summary>
        /// <param name="engine"></param>
        public void Startup(IStartupEngine engine)
        {
            if (_ControllerRegistrationSetup?.EnableControllerRegisterations == true)
                RegisterMvcControllers(engine.Locator, _ControllerRegistrationSetup.ControllerLifeTime);

            DependencyResolver.SetResolver(new NullableMvcDependencyResolver(engine.Locator));
        }

        static void RegisterMvcControllers(ILocator locator, LifeTime controllerLifetime)
        {
            if (locator == null)
                throw new ArgumentNullException($"{nameof(locator)} cannot be null, please install a locator package such as DotNetStarter.DryIoc or DotNetStart.Structuremap!");

            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = locator.Get<IAssemblyScanner>()?.GetTypesFor(typeof(IController));

            foreach (var controller in controllerTypes.Where(x => !x.IsAbstract && !x.IsInterface))
            {
                registry?.Add(controller, controller, lifeTime: controllerLifetime);
            }
        }
    }
}
