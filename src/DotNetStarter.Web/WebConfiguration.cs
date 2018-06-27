using DotNetStarter.Abstractions;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Registers httpcontextbase for scoped requests
    /// </summary>
    [StartupModule]
    public class WebConfiguration : ILocatorConfigure
    {
        /// <summary>
        /// Static bool to disable http context registration, must be set before DotNetStarter.ApplicationContext.Startup!
        /// </summary>
        public static bool RegisterScopedHttpContext { get; set; } = true;

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
#if !NETSTANDARD
            if (RegisterScopedHttpContext)
            {
                registry.Add(typeof(System.Web.HttpContextBase), _ => new System.Web.HttpContextWrapper(System.Web.HttpContext.Current), Lifecycle.Scoped);
            }
#endif
        }
    }
}
