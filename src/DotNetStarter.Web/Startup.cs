#if !NETSTANDARD1_3

using DotNetStarter.Abstractions;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Registers the HttpModule for starting up IHttpModules
    /// </summary>
    [StartupModule]
    public class Startup : IStartupModule
    {
        void IStartupModule.Shutdown(IStartupEngine engine) { }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            try
            {
                var moduleType = typeof(WebModuleStartup);
#if NET40
                // requires Microsoft.Web.Infrastructure package for .net 4.0
                Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(moduleType);
#elif NET45
                System.Web.HttpApplication.RegisterModule(moduleType);
#endif
            }
            catch (System.InvalidOperationException)
            {
#if NET40 || NET45
                throw new System.InvalidOperationException($"Please execute {typeof(ApplicationContext).FullName}.{nameof(ApplicationContext.Startup)} in a {typeof(System.Web.PreApplicationStartMethodAttribute)} startup method or the global asax constructor!");
#endif
            }
        }
    }
}
#endif