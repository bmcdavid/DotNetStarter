using DotNetStarter.Abstractions;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// executes on DotNetStarter startup to set MVC controller builder
    /// </summary>
    [StartupModule]
    public class StartupMvc : IStartupModule
    {
        void IStartupModule.Shutdown(IStartupEngine engine) { }
        
        void IStartupModule.Startup(IStartupEngine engine)
        {
            engine.OnStartupComplete += () => Engine_OnStartupComplete(engine.Locator);
        }

        private void Engine_OnStartupComplete(ILocator locator)
        {
            // sets a default controller factory
            // cannot register an IControllerFactory and set like this, the following exception will be thrown
            // An instance of IControllerFactory was found in the resolver as well as a custom registered provider in ControllerBuilder.GetControllerFactory. Please set only one or the other.
            // ControllerBuilder.Current.SetControllerFactory(new DotNetStarterMvcControllerFactory(locator.Get<IStartupContext>()));

            // sets a default dependency resolver
            DependencyResolver.SetResolver(new NullableMvcDependencyResolver(locator));
        }
    }
}
