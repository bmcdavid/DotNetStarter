using System;
using DotNetStarter.Abstractions;

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
        private readonly ILocatorScopedFactory _LocatorScopeFactory;

        //todo: v2 remove first constructor, and only inject ILocatorScopedFactory

        /// <summary>
        /// Deprecated Constructor
        /// </summary>
        /// <param name="locator"></param>
        [Obsolete]
        public ServiceScopeFactory(ILocator locator) : this(locator, locator.Get<ILocatorScopedFactory>()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="locatorScopedFactory"></param>
        public ServiceScopeFactory(ILocator locator, ILocatorScopedFactory locatorScopedFactory)
        {
            Locator = locator;
            _LocatorScopeFactory = locatorScopedFactory;
        }

        /// <summary>
        /// Creates a scoped service provider
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            var scope = _LocatorScopeFactory.CreateScope();

            return scope.Get<IServiceScope>();
        }
    }
}