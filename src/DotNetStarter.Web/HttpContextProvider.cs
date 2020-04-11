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
        private static readonly string key =
            $"{nameof(DotNetStarter)}.{nameof(IHttpContextProvider)}";

        /// <summary>
        /// Provides access to current HttpContext
        /// </summary>
        public virtual HttpContextBase CurrentContext
        {
            get
            {
                if (!(HttpContext.Current is HttpContext current)) { return null; }

                if (!(current.Items[key] is HttpContextBase httpContextBase))
                {
                    current.Items[key] = httpContextBase = new HttpContextWrapper(current);
                }

                return httpContextBase;
            }
        }
    }
}