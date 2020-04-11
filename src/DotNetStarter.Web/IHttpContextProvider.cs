using System.Web;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Provides access to current HttpContext
    /// </summary>
    public interface IHttpContextProvider
    {
        /// <summary>
        /// Current HttpContext, best performance to read once per method
        /// </summary>
        HttpContextBase CurrentContext { get; }
    }
}