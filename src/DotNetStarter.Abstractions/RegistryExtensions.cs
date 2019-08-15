namespace DotNetStarter.Abstractions
{
    using DotNetStarter.Abstractions.Internal;
    using System;

    /// <summary>
    /// Extensions for adding services
    /// </summary>
    public static class RegistryExtensions
    {
        private static void GuardIfNull(this Type t, string argName)
        {
            if (t is null) { throw new ArgumentNullException(argName); }
        }

        /// <summary>
        /// Adds an instance
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="registry"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static ILocatorRegistry AddInstance<TService>(this ILocatorRegistry registry, TService instance)
        {
            registry.Add(typeof(TService), instance);
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
        public static ILocatorRegistry AddTransient<TService, TImplementation>(this ILocatorRegistry registry) where TImplementation : TService
        {
            registry.Add<TService, TImplementation>(lifecycle: Lifecycle.Transient);
            return registry;
        }

        /// <summary>
        /// Confirms that serviceImplementation can be used as serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        public static void ConfirmService(Type serviceType, Type serviceImplementation)
        {
            serviceImplementation.GuardIfNull(nameof(serviceImplementation));
            serviceType.GuardIfNull(nameof(serviceType));

            if (serviceType.IsAssignableFromCheck(serviceImplementation))
            {
                return;
            }

            if (!serviceType.IsGenericType() || 
                !serviceImplementation.IsGenericInterface(serviceType))
            {
                ThrowRegisterException(serviceType, serviceImplementation);
            }
        }

        private static void ThrowRegisterException(Type service, Type implementation)
        {
            throw new ArgumentException($"{implementation.FullName} cannot be converted to {service.FullName}!");
        }
    }
}