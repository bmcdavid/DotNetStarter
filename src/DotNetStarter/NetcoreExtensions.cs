#if NETSTANDARD1_0 || NETSTANDARD1_1

namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// Netcore extensions
    /// </summary>
    public static class NetcoreExtensions
    {
        /// <summary>
        /// Registers service collection with DotNetStarter
        /// <para>IMPORTANT: This should be ran before executing DotNetStarter.ApplicationContext.Startup</para>
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void WithDotNetStarter(this IServiceCollection serviceCollection)
        {
            RegisterServiceCollection.Services = serviceCollection;
        }

        /// <summary>
        /// Registers service collection with DotNetStarter
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="startupDotNetStarter">delegate function to invoke DotNetStarter.ApplicationContext.Startup</param>
        /// <param name="serviceProviderFactory">delegate function to create an IServiceProvider</param>
        public static IServiceProvider WithDotNetStarter(this IServiceCollection serviceCollection, Action startupDotNetStarter, Func<ILocator,IServiceProvider> serviceProviderFactory = null)
        {
            if (startupDotNetStarter == null)
                throw new ArgumentNullException(nameof(startupDotNetStarter));

            Func<ILocator, IServiceProvider> fallbackFactory = locator => locator.Get<IServiceProvider>();

            WithDotNetStarter(serviceCollection); // add services
            startupDotNetStarter();

            return (serviceProviderFactory ?? fallbackFactory).Invoke(ApplicationContext.Default.Locator);
        }
    }

    /// <summary>
    /// Adds service collection to the ILocator during locator configure
    /// </summary>
    [StartupModule(typeof(RegisterConfiguration), typeof(RegistrationConfiguration))]
    public class RegisterServiceCollection : ILocatorConfigure
    {
        internal static IServiceCollection Services { get; set; }

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            if (Services != null)
            {
                AddServicesToLocator(Services, registry);
            }
        }

        void AddServicesToLocator(IServiceCollection services, ILocatorRegistry locator)
        {
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
                default:
                    return LifeTime.Transient;
            }
        }
    }
}
#endif