namespace DotNetStarter.Web
{
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Main startup module
    /// </summary>
    public class WebModuleStartup : IHttpModule
    {
        private IEnumerable<IHttpModule> StartupModules;

        Import<IWebModuleStartupHandler> StartupWebModuleHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        public WebModuleStartup() { }

        /// <summary>
        /// Mockable constructor
        /// </summary>
        /// <param name="httpModules"></param>
        public WebModuleStartup(IEnumerable<IHttpModule> httpModules)
        {
            StartupModules = httpModules;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Initialize modules
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            if (StartupWebModuleHandler.Service.Enabled())
            {
                StartupWebModuleHandler.Service.Startup(context, StartupModules);
            }
        }
    }
}