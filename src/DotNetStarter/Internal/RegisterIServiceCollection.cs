#if NETSTANDARD
using DotNetStarter.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Non discoverable IServiceCollection registration
    /// </summary>
    public class RegisterIServiceCollection : ILocatorConfigure
    {
        private readonly IServiceCollection _serviceCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceCollection"></param>
        public RegisterIServiceCollection(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        void ILocatorConfigure.Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            AddServicesToLocator(_serviceCollection, registry);
        }

        internal static void AddServicesToLocator(IServiceCollection services, ILocatorRegistry locator)
        {
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                var lifetime = ConvertLifeTime(service.Lifetime);

                if (service.ImplementationType is object)
                {
                    locator.Add(service.ServiceType, service.ImplementationType, lifecycle: lifetime);
                }
                else if (service.ImplementationFactory is object)
                {
                    locator.Add(service.ServiceType,
                    l => service.ImplementationFactory(l.GetServiceProvider()),
                    lifetime);
                }
                else
                {
                    locator.Add(service.ServiceType, service.ImplementationInstance);
                }
            }
        }

        internal static Lifecycle ConvertLifeTime(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    return Lifecycle.Scoped;

                case ServiceLifetime.Singleton:
                    return Lifecycle.Singleton;

                case ServiceLifetime.Transient:
                default:
                    return Lifecycle.Transient;
            }
        }
    }
}
#endif