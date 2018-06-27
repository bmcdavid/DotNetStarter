#if !NETSTANDARD
namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System.Web;
    using static ApplicationContext;

    /// <summary>
    /// HttpContext extensions for System.Web
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets a scoped locator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILocator GetScopedLocator(this HttpContext context)
        {
            return context?.Items[ScopedLocatorKeyInContext] as ILocator;
        }

        /// <summary>
        /// Gets a scoped locator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILocator GetScopedLocator(this HttpContextBase context)
        {
            return context?.Items[ScopedLocatorKeyInContext] as ILocator;
        }
    }
}
#endif
