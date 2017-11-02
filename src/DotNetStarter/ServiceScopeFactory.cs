using System;
using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;

#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD2_0
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter
{
#if NET45 || NET35 || NET40
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
    [Register(typeof(IServiceScopeFactory), LifeTime.Scoped)]
    public class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ILocator Locator;
        private readonly ILocatorScopeFactory _LocatorScopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        public ServiceScopeFactory(ILocator locator)
        {
            Locator = locator;
            //_LocatorScopeFactory = locatorScopeFactory;
        }

        /// <summary>
        /// Creates a scoped service provider
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            ILocator scope = null;
            //scope = _LocatorScopeFactory.CreateScope(null);
            //var providerCreator = scope.Get<Func<ILocator, IServiceProvider>>();

            //return new ServiceScope(providerCreator.Invoke(scope));

            scope = Locator.OpenScope();
            (scope as ILocatorRegistry).Add(typeof(ILocator), scope);// add scope locator to be resolved so root container isn't disposed

            return new ServiceScope(scope.Get<IServiceProvider>());
        }
    }
}