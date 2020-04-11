using DotNetStarter.Abstractions;
using System.Web;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Registers the HttpModule for starting up IHttpModules
    /// </summary>
    [StartupModule]
    public class Startup : IStartupModule
    {
        private static bool IsRegistered = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorAmbient"></param>
        public Startup(ILocatorAmbient locatorAmbient)
        {
            locatorAmbient.PreferredStorageAccessor(() => HttpContext.Current?.Items);
        }

        void IStartupModule.Shutdown() { }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            RegisterWebModuleStartup();
        }

        /// <summary>
        /// Registers the main DotNetStarter WebModuleStartup for netframework
        /// </summary>
        public static void RegisterWebModuleStartup()
        {
            if (IsRegistered) { return; }

            try
            {
                var moduleType = typeof(WebModuleStartup);

                HttpApplication.RegisterModule(moduleType);
            }
            catch (System.InvalidOperationException)
            {
                throw new System.InvalidOperationException($"Please execute {typeof(Configure.StartupBuilder).FullName} in a {typeof(PreApplicationStartMethodAttribute)} startup method or the global asax constructor!");
            }

            IsRegistered = true;
        }
    }
}