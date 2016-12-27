using System.Collections.Generic;
using System.Web;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Handles IHttpModule Init
    /// </summary>
    public interface IWebModuleStartupHandler
    {
        /// <summary>
        /// Determines if startup is executed
        /// </summary>
        /// <returns></returns>
        bool Enabled();

        /// <summary>
        /// Initialize modules
        /// </summary>
        /// <param name="context"></param>
        /// <param name="startupModules"></param>
        void Startup(HttpApplication context, IEnumerable<IHttpModule> startupModules);
    }
}