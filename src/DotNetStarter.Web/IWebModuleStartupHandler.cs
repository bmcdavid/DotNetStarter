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
        bool StartupEnabled();

        /// <summary>
        /// Determines if startup handler creates a scoped ILocator stored in HttpContext.Items
        /// </summary>
        /// <returns></returns>
        bool ScopeEnabled();

        /// <summary>
        /// Initialize modules
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="startupModules"></param>
        void Startup(HttpApplication applicationContext, IEnumerable<IHttpModule> startupModules);

        /// <summary>
        /// Opens locator scope
        /// </summary>
        /// <param name="applicationContext"></param>
        void OpenLocatorScope(HttpApplication applicationContext);
    }
}