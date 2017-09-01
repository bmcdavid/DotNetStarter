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
        private IControllerFactory _ControllerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllerFactory"></param>
        public StartupMvc(IControllerFactory controllerFactory)
        {
            _ControllerFactory = controllerFactory;
        }

        void IStartupModule.Shutdown(IStartupEngine engine) { }
        
        void IStartupModule.Startup(IStartupEngine engine)
        {
            ControllerBuilder.Current.SetControllerFactory(_ControllerFactory);
        }        
    }
}
