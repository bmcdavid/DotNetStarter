#if !NETSTANDARD

using DotNetStarter.Abstractions;
using System.Web;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Provides access to current HttpContext
    /// </summary>
    [Registration(typeof(IHttpContextProvider), Lifecycle.Singleton)]
    public class HttpContextProvider : IHttpContextProvider
    {
        /// <summary>
        /// Provides access to current HttpContext
        /// </summary>
        public virtual HttpContextBase CurrentContext => new HttpContextWrapper(HttpContext.Current);
    }
}

#endif