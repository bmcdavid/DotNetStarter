﻿using System;
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
    /// Creates IServiceScopeFactory
    /// </summary>
    [Registration(typeof(IServiceScopeFactory), Lifecycle.Singleton)]
    public class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ILocatorScopedFactory _LocatorScopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorScopedFactory"></param>
        public ServiceScopeFactory(ILocatorScopedFactory locatorScopedFactory)
        {
            _LocatorScopeFactory = locatorScopedFactory;
        }

        /// <summary>
        /// Creates a scoped service provider
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            var scope = _LocatorScopeFactory.CreateScope();

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