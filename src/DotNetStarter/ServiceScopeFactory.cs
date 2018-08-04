using DotNetStarter.Abstractions;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter
{
#if NETFULLFRAMEWORK && !NETSTANDARD

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
    /// Creates IServiceScopeFactory
    /// </summary>
    [Registration(typeof(IServiceScopeFactory), Lifecycle.Singleton)]
    public class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ILocatorScopedFactory _locatorScopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorScopedFactory"></param>
        public ServiceScopeFactory(ILocatorScopedFactory locatorScopedFactory)
        {
            _locatorScopeFactory = locatorScopedFactory;
        }

        /// <summary>
        /// Creates a scoped service provider
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            var scope = _locatorScopeFactory.CreateScope();

            return new ServiceScope
            (
                new ServiceProvider
                (
                    scope.Get<IServiceProviderTypeChecker>(),
                    scope.Get<ILocatorScopedAccessor>()
                )
            );
        }
    }
}