using System;
using System.Web;
using DotNetStarter.Web;

[assembly: PreApplicationStartMethod(typeof(Startup), nameof(Startup.WebModules))]

namespace DotNetStarter.Web
{
    /// <summary>
    /// Fires way before Application_Start in global.asax
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Registers module without need for web.config entry
        /// </summary>
        public static void WebModules()
        {
            RegisterModule(typeof(WebModuleStartup));
        }

        private static void RegisterModule(Type moduleType)
        {
#if NET40
            // requires Microsoft.Web.Infrastructure package for .net 4.0
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(moduleType);
#else
            HttpApplication.RegisterModule(moduleType);
#endif
        }
    }
}
