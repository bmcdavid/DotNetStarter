using DotNetStarter.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetStarter
{
    /// <summary>
    /// Netcore extensions
    /// </summary>
    public static class NetcoreExtensions
    {
        // todo: v2, create a package for netcore dependency injection

        // todo: v2, figure out a way separate startup and locator setup

        /// <summary>
        /// Adds service collection items to locator
        /// </summary>
        /// <param name="services"></param>
        /// <param name="locator"></param>
        public static IServiceProvider AddServicesToLocator(this IServiceCollection services, ILocatorRegistry locator)
        {
            if (locator == null)
                throw new ArgumentNullException();

            locator.Add<IServiceProvider, ServiceProvider>(lifetime: LifeTime.Scoped);

            // Scope factory should be scoped itself to enable nested scopes creation
            locator.Add<IServiceScopeFactory, ServiceScopeFactory>(lifetime: LifeTime.Scoped);

            // map .net services to locator
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                var lifetime = ConvertLifeTime(service.Lifetime);

                if (service.ImplementationType != null)
                {
                    locator.Add(service.ServiceType, service.ImplementationType, lifeTime: lifetime);
                }
                else if (service.ImplementationFactory != null)
                {
                    locator.Add(service.ServiceType, l => service.ImplementationFactory(l.Get<IServiceProvider>()), lifetime);
                }
                else
                {
                    locator.Add(service.ServiceType, service.ImplementationInstance);
                }
            }

            return locator.Get<IServiceProvider>();
        }

        private static LifeTime ConvertLifeTime(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    return LifeTime.Scoped;
                case ServiceLifetime.Singleton:
                    return LifeTime.Singleton;
                case ServiceLifetime.Transient:
                    return LifeTime.Transient;
            }

            return LifeTime.Transient;
        }
    }
}
