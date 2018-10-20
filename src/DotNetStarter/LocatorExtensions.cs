using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter
{
    /// <summary>
    /// Locator extensions for application developers
    /// </summary>
    public static class LocatorExtensions
    {
        /// <summary>
        /// Attempts to see if given ILocator implementions IServiceProvider, otherwise creates one from the ILocatorAmbient
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IServiceProvider GetServiceProvider(this ILocator locator)
        {
            if (locator is ILocatorScoped && locator is IServiceProvider) return locator as IServiceProvider;
            var ambient = locator.Get<ILocatorAmbient>();

            return ambient.Current is IServiceProvider serviceProvider
                ? serviceProvider
                : new DotNetStarter.ServiceProvider(ambient, locator.Get<IServiceProviderTypeChecker>(), locator.Get<IStartupConfiguration>());
        }
    }
}