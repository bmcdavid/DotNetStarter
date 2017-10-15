#if !NETSTANDARD1_3

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

        private IWebModuleStartupHandler _ModuleHandler;

        /// <summary>
        /// IWebModuleStartupHandler fallback instance
        /// </summary>
        public Import<IWebModuleStartupHandler> StartupWebModuleHandler { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebModuleStartup() : this(null, null) { }

        /// <summary>
        /// Mockable constructor
        /// </summary>
        /// <param name="httpModules"></param>
        /// <param name="moduleHandler"></param>
        public WebModuleStartup(IEnumerable<IHttpModule> httpModules, IWebModuleStartupHandler moduleHandler)
        {
            StartupModules = httpModules;
            _ModuleHandler = moduleHandler;
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
            if (_ModuleHandler == null)
                _ModuleHandler = StartupWebModuleHandler.Service;

            if (_ModuleHandler.ScopeEnabled())
                _ModuleHandler.OpenLocatorScope(application);

            if (_ModuleHandler.StartupEnabled())
                _ModuleHandler.Startup(application, StartupModules);

        }
    }
}
#endif