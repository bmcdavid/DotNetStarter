namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Extensions for adding services
    /// </summary>
    public static class RegistryExtensions
    {
        /// <summary>
        /// Adds a transient lifecycle service
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddTransient(this ILocatorRegistry registry, Type serviceType, Type implementationType)
        {
            registry.Add(serviceType, implementationType, lifecycle: Lifecycle.Transient);
            return registry;
        }

        /// <summary>
        /// Adds a transient lifecycle service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddTransient<TService,TImplementation>(this ILocatorRegistry registry) where TImplementation : TService
        {
            registry.Add<TService,TImplementation>(lifecycle: Lifecycle.Transient);
            return registry;
        }

        /// <summary>
        /// Adds a scoped lifecycle service
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddScoped(this ILocatorRegistry registry, Type serviceType, Type implementationType)
        {
            registry.Add(serviceType, implementationType, lifecycle: Lifecycle.Scoped);
            return registry;
        }

        /// <summary>
        /// Adds a scoped lifecycle service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddScoped<TService, TImplementation>(this ILocatorRegistry registry) where TImplementation : TService
        {
            registry.Add<TService, TImplementation>(lifecycle: Lifecycle.Scoped);
            return registry;
        }

        /// <summary>
        /// Adds a singleton lifecycle service
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddSingleton(this ILocatorRegistry registry, Type serviceType, Type implementationType)
        {
            registry.Add(serviceType, implementationType, lifecycle: Lifecycle.Singleton);
            return registry;
        }

        /// <summary>
        /// Adds a singleton lifecycle service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddSingleton<TService, TImplementation>(this ILocatorRegistry registry) where TImplementation : TService
        {
            registry.Add<TService, TImplementation>(lifecycle: Lifecycle.Singleton);
            return registry;
        }
    }
}