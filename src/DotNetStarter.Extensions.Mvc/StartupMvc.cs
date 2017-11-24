using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// executes on DotNetStarter startup to set MVC controller builder
    /// </summary>
    [StartupModule]
    public class StartupMvc : IStartupModule
    {
        private readonly ILocator _Locator;
        private readonly IServiceProviderTypeChecker _ServiceProviderTypeChecker;
        private readonly IHttpContextProvider _HttpContextProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="serviceProviderTypeChecker"></param>
        /// <param name="httpContextProvider"></param>
        public StartupMvc(ILocator locator, IServiceProviderTypeChecker serviceProviderTypeChecker, IHttpContextProvider httpContextProvider)
        {
            _Locator = locator;
            _ServiceProviderTypeChecker = serviceProviderTypeChecker;
            _HttpContextProvider = httpContextProvider;
        }

        void IStartupModule.Shutdown() { }
        
        void IStartupModule.Startup(IStartupEngine engine)
        {
            engine.OnStartupComplete += () => Engine_OnStartupComplete();
        }

        private void Engine_OnStartupComplete()
        {
            // cannot register an IControllerFactory and set like this, the following exception will be thrown
            // An instance of IControllerFactory was found in the resolver as well as a custom registered provider in ControllerBuilder.GetControllerFactory. Please set only one or the other.            

            // sets a default dependency resolver
            DependencyResolver.SetResolver(new ScopedDependencyResolver(_Locator, _ServiceProviderTypeChecker, _HttpContextProvider));
        }
    }
}
