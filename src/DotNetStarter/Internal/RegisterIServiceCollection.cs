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

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngineConfigurationArgs engine)
        {
            AddServicesToLocator(_serviceCollection, registry);
        }

        internal static void AddServicesToLocator(IServiceCollection services, ILocatorRegistry locator)
        {
            // map .net services to locator
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                var lifetime = ConvertLifeTime(service.Lifetime);

                if (service.ImplementationType != null)
                {
                    locator.Add(service.ServiceType, service.ImplementationType, lifecycle: lifetime);
                }
                else if (service.ImplementationFactory != null)
                {
                    locator.Add(service.ServiceType,
                    l =>
                    {
                        var provider = new ServiceProvider(l.Get<ILocatorAmbient>(), l.Get<IServiceProviderTypeChecker>(), l.Get<IStartupConfiguration>());
                        return service.ImplementationFactory(provider);
                    },
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