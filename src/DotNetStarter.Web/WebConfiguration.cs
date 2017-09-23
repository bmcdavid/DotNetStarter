using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Registers httpcontextbase for scoped requests
    /// </summary>
    [StartupModule]
    public class WebConfiguration : ILocatorConfigure
    {
        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
#if !NETSTANDARD1_3
            registry.Add<IServiceProvider, ServiceProvider>(lifetime: LifeTime.Scoped);
            registry.Add(typeof(System.Web.HttpContextBase), _ => new System.Web.HttpContextWrapper(System.Web.HttpContext.Current), LifeTime.Scoped);
#endif
        }
    }
}
