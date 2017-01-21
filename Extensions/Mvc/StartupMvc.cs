using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DotNetStarter.Mvc
{
    /// <summary>
    /// executes on DotNetStarter startup to set MVC dependency resolver and register controllers
    /// </summary>
    [StartupModule]
    public class StartupMvc : IStartupModule
    {
        ILocator _Locator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        public StartupMvc(ILocator locator)
        {
            _Locator = locator;
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
            RegisterMvcControllers(engine.Locator);

            DependencyResolver.SetResolver(new ScopedDependencyResolver(engine.Locator));
        }

        static void RegisterMvcControllers(ILocator locator)
        {
            if (locator == null)
                throw new ArgumentNullException($"{nameof(locator)} cannot be null, please install a locator package such as DotNetStarter.DryIoc or DotNetStart.Structuremap!");

            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = locator.Get<IAssemblyScanner>()?.GetTypesFor(typeof(IController));

            foreach (var controller in controllerTypes.Where(x => !x.IsAbstract && !x.IsInterface))
            {
                registry?.Add(controller, controller, lifeTime: LifeTime.Scoped);
            }
        }
    }
}
