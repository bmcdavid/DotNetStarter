namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Initializes IHttpModules
    /// </summary>
    [Register(typeof(IWebModuleStartupHandler), LifeTime.Singleton)]
    public class WebModuleStartupHandler : IWebModuleStartupHandler
    {
        private IStartupContext _StartupContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public WebModuleStartupHandler(IStartupContext context)
        {
            _StartupContext = context;
        }
        
        /// <summary>
        /// Default is true.
        /// </summary>
        /// <returns></returns>
        public virtual bool Enabled() => true;

        /// <summary>
        /// Initializes modules
        /// </summary>
        /// <param name="context"></param>
        /// <param name="startupModules"></param>
        public virtual void Startup(HttpApplication context, IEnumerable<IHttpModule> startupModules)
        {
            startupModules = startupModules ?? _StartupContext.Locator.GetAll<IStartupModule>().OfType<IHttpModule>();

            foreach(var module in startupModules)
            {
                module.Init(context);
            }
        }
    }
}