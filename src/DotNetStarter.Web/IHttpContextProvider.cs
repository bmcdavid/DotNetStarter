using System.Web;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Provides access to current HttpContext
    /// </summary>
    public interface IHttpContextProvider
    {
        /// <summary>
        /// Current HttpContext
        /// </summary>
        HttpContextBase CurrentContext { get; }
    }
}