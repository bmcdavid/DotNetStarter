namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System;
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
        /// <param name="application"></param>
        public void Init(HttpApplication application)
        {
            var handler = StartupWebModuleHandler.Service;

            if (handler == null)
                throw new ArgumentNullException();

            if (handler.ScopeEnabled())
                handler.OpenLocatorScope(application);

            if (handler.StartupEnabled())
                handler.Startup(application, StartupModules);

        }
    }
}