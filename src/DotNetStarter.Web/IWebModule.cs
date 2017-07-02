#if !NETSTANDARD1_3

using DotNetStarter.Abstractions;
using System.Web;

namespace DotNetStarter.Web.Abstractions
{
    /// <summary>
    /// Startup Web Module
    /// </summary>
    public interface IWebModule : IHttpModule, IStartupModule
    {
    }
}
#endif