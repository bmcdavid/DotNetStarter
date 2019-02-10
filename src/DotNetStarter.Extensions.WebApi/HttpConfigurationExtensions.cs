using DotNetStarter.Abstractions;
using System.Web.Http;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Extensions to for WebApi
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Assigns a WebApi Dependency Resolver to given configuration
        /// </summary>
        /// <param name="httpConfiguration"></param>
        /// <param name="locator"></param>
        public static void AssignDotNetStarterDependencyResolver(this HttpConfiguration httpConfiguration, ILocator locator)
        {
            httpConfiguration.DependencyResolver =
                new WebApiDependencyResolver(locator); // never call DotNetStarter.ApplicationContext.Default.Locator in an IStartupModule
        }
    }
}
