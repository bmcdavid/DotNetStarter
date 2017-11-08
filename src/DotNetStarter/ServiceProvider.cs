namespace DotNetStarter
{
    using Abstractions;
    using System;

#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD2_0
    using Microsoft.Extensions.DependencyInjection;
#endif

#if NET45 || NET40 || NET35
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
    [Register(typeof(IServiceProvider), LifeTime.Scoped)]
    public class ServiceProvider : IServiceProvider, IDisposable, ISupportRequiredService
    {
        /// <summary>
        /// Scoped locator reference
        /// </summary>
        public ILocator Locator { get; }

        private readonly IServiceProviderTypeChecker _ServiceProviderTypeChecker;

        //todo: v2 remove old constructors
    
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        [Obsolete]
        public ServiceProvider(ILocator locator) : this(locator.Get<IServiceProviderTypeChecker>(), locator.Get<ILocatorScopedAccessor>())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProviderTypeChecker"></param>
        /// <param name="locatorScopedAccessor"></param>
        public ServiceProvider(IServiceProviderTypeChecker serviceProviderTypeChecker, ILocatorScopedAccessor locatorScopedAccessor)
        {
            Locator = locatorScopedAccessor.CurrentScope;
            _ServiceProviderTypeChecker = serviceProviderTypeChecker;
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
                if (_ServiceProviderTypeChecker.IsScannedAssembly(serviceType, e))
                {
                    throw;
                }

                return null;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Locator?.Dispose();
        }

        /// <summary>
        /// Gets a service or throws exception if one is not found
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetRequiredService(Type serviceType)
        {
            var service = Locator.Get(serviceType);

            if (service == null)
                throw new NullReferenceException($"{serviceType.FullName} cannot be null and couldn't be resolved!");

            return service;
        }
    }
}
