namespace DotNetStarter.Owin
{
    using DotNetStarter.Abstractions;
    using System;

#if NETSTANDARD
    using Microsoft.Extensions.DependencyInjection;
#endif

#if NET45
    /// <summary>
    /// Serive provider that throws exceptions if type cannot be found
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
    public class ServiceProvider : IServiceProvider, IDisposable, ISupportRequiredService
    {
        /// <summary>
        /// Scoped locator reference
        /// </summary>
        public ILocator Locator { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        public ServiceProvider(ILocator locator)
        {
            Locator = locator;
        }

        /// <summary>
        /// Get service by type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType) => Locator.Get(serviceType);

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
                throw new NullReferenceException();

            return service;
        }
    }
}
