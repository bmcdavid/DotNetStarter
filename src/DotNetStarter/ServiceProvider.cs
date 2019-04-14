namespace DotNetStarter
{
    using Abstractions;
    using Abstractions.Internal;
    using System;

#if NETSTANDARD
    using Microsoft.Extensions.DependencyInjection;
#endif

#if NETFULLFRAMEWORK && !NETSTANDARD

    /// <summary>
    /// Service provider that throws exceptions if type cannot be found
    /// </summary>
    public interface ISupportRequiredService
    {
        /// <summary>
        /// Gets service or throws exception if not found
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        object GetRequiredService(Type serviceType);
    }

#endif

    /// <summary>
    /// Wraps ILocator in a service provider
    /// </summary>
    [Registration(typeof(IServiceProvider), Lifecycle.Singleton)]
    public class ServiceProvider : IServiceProvider, IDisposable, ISupportRequiredService
    {
        /// <summary>
        /// Scoped locator reference
        /// </summary>
        public ILocator Locator => _scopeAccessor?.CurrentScope ?? _locatorAmbient.Current;

        private readonly IServiceProviderTypeChecker _serviceProviderTypeChecker;
        private readonly ILocatorScopedAccessor _scopeAccessor;
        private readonly ILocatorAmbient _locatorAmbient;

        /// <summary>
        /// Scoped Constructor for OWIN and netcore
        /// </summary>
        /// <param name="serviceProviderTypeChecker"></param>
        /// <param name="locatorScopedAccessor"></param>
        public ServiceProvider(IServiceProviderTypeChecker serviceProviderTypeChecker, ILocatorScopedAccessor locatorScopedAccessor)
        {
            _scopeAccessor = locatorScopedAccessor;
            _serviceProviderTypeChecker = serviceProviderTypeChecker;
        }

        /// <summary>
        /// Singleton Constructor, greediest one to use
        /// </summary>
        /// <param name="locatorAmbient"></param>
        /// <param name="serviceProviderTypeChecker"></param>
        /// <param name="startupConfiguration"></param>
        public ServiceProvider(ILocatorAmbient locatorAmbient, IServiceProviderTypeChecker serviceProviderTypeChecker, IStartupConfiguration startupConfiguration)
        {
            _locatorAmbient = locatorAmbient;
            _serviceProviderTypeChecker = serviceProviderTypeChecker;
        }

        /// <summary>
        /// Get service by type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType)
        {
            try
            {
                return Locator.Get(serviceType);
            }
            catch (Exception e)
            {
                if (_serviceProviderTypeChecker.IsScannedAssembly(serviceType, e))
                {
                    throw;
                }

                if (serviceType.IsAbstract() || serviceType.IsInterface())
                {
                    return null;
                }

                return Activator.CreateInstance(serviceType);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_scopeAccessor?.CurrentScope is object)
            {
                _scopeAccessor?.CurrentScope.Dispose();
            }

            if (_locatorAmbient?.Current is ILocatorScoped scoped)
            {
                scoped.Dispose();
            }
        }

        /// <summary>
        /// Gets a service or throws exception if one is not found
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetRequiredService(Type serviceType)
        {
            var service = Locator.Get(serviceType);

            if (service is null)
            {
                throw new NullReferenceException($"{serviceType.FullName} cannot be null and couldn't be resolved!");
            }

            return service;
        }
    }
}