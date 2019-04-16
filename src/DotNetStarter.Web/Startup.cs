using DotNetStarter.Abstractions;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Registers the HttpModule for starting up IHttpModules
    /// </summary>
    [StartupModule]
    public class Startup : IStartupModule
    {
        private static bool IsRegistered = false;

        void IStartupModule.Shutdown()
        {
        }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            RegisterWebModuleStartup();
        }

        /// <summary>
        /// Registers the main DotNetStarter WebModuleStartup for netframework
        /// </summary>
        public static void RegisterWebModuleStartup()
        {
            if (IsRegistered)
            {
                return;
            }

            try
            {
                var moduleType = typeof(WebModuleStartup);

                System.Web.HttpApplication.RegisterModule(moduleType);
            }
            catch (System.InvalidOperationException)
            {
                throw new System.InvalidOperationException($"Please execute {typeof(DotNetStarter.Configure.StartupBuilder).FullName} in a {typeof(System.Web.PreApplicationStartMethodAttribute)} startup method or the global asax constructor!");
            }

            IsRegistered = true;
        }
    }
}