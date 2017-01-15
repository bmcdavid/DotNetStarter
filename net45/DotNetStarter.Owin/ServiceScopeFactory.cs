using System;
using DotNetStarter.Abstractions;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter.Owin
{
#if NET45
    /// <summary>
    /// Factory to create scoped service providers
    /// </summary>
    public interface IServiceScopeFactory
    {
        /// <summary>
        /// Creates a scope
        /// </summary>
        /// <returns></returns>
        IServiceScope CreateScope();
    }
#endif

    /// <summary>
    /// Creates a scoped service provider with injected locator
    /// </summary>
    public class ServiceScopeFactory : IServiceScopeFactory
    {
        ILocator Locator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        public ServiceScopeFactory(ILocator locator)
        {
            Locator = locator;
        }

        /// <summary>
        /// Creates a scoped service provider
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            var scope = Locator.OpenScope();

            return new ServiceScope(scope.Get<IServiceProvider>());
        }
    }
}
