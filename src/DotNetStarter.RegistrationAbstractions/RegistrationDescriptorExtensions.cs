// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/DependencyInjection/blob/master/LICENSE.txt for license information.
// modified by Brad McDavid 2018

using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Extension methods for adding and removing services to an <see cref="IRegistrationCollection" />.
    /// </summary>
    public static class RegistrationCollectionDescriptorExtensions
    {
        /// <summary>
        /// Adds the specified <paramref name="descriptor"/> to the <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="descriptor">The <see cref="RegistrationDescriptor"/> to add.</param>
        /// <returns>A reference to the current instance of <see cref="IRegistrationCollection"/>.</returns>
        public static IRegistrationCollection Add(
            this IRegistrationCollection collection,
            RegistrationDescriptor descriptor)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            collection.Add(descriptor);
            return collection;
        }

        /// <summary>
        /// Adds a sequence of <see cref="RegistrationDescriptor"/> to the <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="descriptors">The <see cref="RegistrationDescriptor"/>s to add.</param>
        /// <returns>A reference to the current instance of <see cref="IRegistrationCollection"/>.</returns>
        public static IRegistrationCollection Add(
            this IRegistrationCollection collection,
            IEnumerable<RegistrationDescriptor> descriptors)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptors is null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            foreach (var descriptor in descriptors)
            {
                collection.Add(descriptor);
            }

            return collection;
        }

        /// <summary>
        /// Adds an instance
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="registry"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddInstance<TService>(this IRegistrationCollection registry, TService instance)
        {
            var descriptor = RegistrationDescriptor.Singleton(typeof(TService), instance);
            registry.Add(descriptor);
            return registry;
        }

        /// <summary>
        /// Adds a scoped lifecycle service
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddScoped(this IRegistrationCollection registry, Type serviceType, Type implementationType)
        {
            registry.Add(RegistrationDescriptor.Scoped(serviceType, implementationType));
            return registry;
        }

        /// <summary>
        /// Adds a scoped lifecycle service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddScoped<TService, TImplementation>(this IRegistrationCollection registry) where TImplementation : TService
        {
            registry.Add(RegistrationDescriptor.Scoped(typeof(TService), typeof(TImplementation)));
            return registry;
        }

        /// <summary>
        /// Adds a singleton lifecycle service
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddSingleton(this IRegistrationCollection registry, Type serviceType, Type implementationType)
        {
            registry.Add(RegistrationDescriptor.Singleton(serviceType, implementationType));
            return registry;
        }

        /// <summary>
        /// Adds a singleton lifecycle service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddSingleton<TService, TImplementation>(this IRegistrationCollection registry) where TImplementation : TService
        {
            registry.Add(RegistrationDescriptor.Singleton(typeof(TService), typeof(TImplementation)));
            return registry;
        }

        /// <summary>
        /// Adds a transient lifecycle service
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddTransient(this IRegistrationCollection registry, Type serviceType, Type implementationType)
        {
            registry.Add(RegistrationDescriptor.Transient(serviceType, implementationType));
            return registry;
        }

        /// <summary>
        /// Adds a transient lifecycle service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static IRegistrationCollection AddTransient<TService, TImplementation>(this IRegistrationCollection registry) where TImplementation : TService
        {
            registry.Add(RegistrationDescriptor.Transient(typeof(TService), typeof(TImplementation)));
            return registry;
        }

        /// <summary>
        /// Removes all services of type <typeparamef name="T"/> in <see cref="IRegistrationCollection"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <returns></returns>
        public static IRegistrationCollection RemoveAll<T>(this IRegistrationCollection collection)
        {
            return RemoveAll(collection, typeof(T));
        }

        /// <summary>
        /// Removes all services of type <paramef name="serviceType"/> in <see cref="IRegistrationCollection"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="serviceType">The service type to remove.</param>
        /// <returns></returns>
        public static IRegistrationCollection RemoveAll(this IRegistrationCollection collection, Type serviceType)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            for (var i = collection.Count - 1; i >= 0; i--)
            {
                var descriptor = collection[i];
                if (descriptor.ServiceType == serviceType)
                {
                    collection.RemoveAt(i);
                }
            }

            return collection;
        }

        /// <summary>
        /// Removes the first service in <see cref="IRegistrationCollection"/> with the same service type
        /// as <paramref name="descriptor"/> and adds <paramef name="descriptor"/> to the collection.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="descriptor">The <see cref="RegistrationDescriptor"/> to replace with.</param>
        /// <returns></returns>
        public static IRegistrationCollection Replace(
            this IRegistrationCollection collection,
            RegistrationDescriptor descriptor)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            var registeredRegistrationDescriptor = collection.FirstOrDefault(s => s.ServiceType == descriptor.ServiceType);
            if (registeredRegistrationDescriptor is object)
            {
                collection.Remove(registeredRegistrationDescriptor);
            }

            collection.Add(descriptor);
            return collection;
        }

        /// <summary>
        /// Adds the specified <paramref name="descriptor"/> to the <paramref name="collection"/> if the
        /// service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="descriptor">The <see cref="RegistrationDescriptor"/> to add.</param>
        public static void TryAdd(
            this IRegistrationCollection collection,
            RegistrationDescriptor descriptor)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (!collection.Any(d => d.ServiceType == descriptor.ServiceType))
            {
                collection.Add(descriptor);
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="descriptors"/> to the <paramref name="collection"/> if the
        /// service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="descriptors">The <see cref="RegistrationDescriptor"/>s to add.</param>
        public static void TryAdd(
            this IRegistrationCollection collection,
            IEnumerable<RegistrationDescriptor> descriptors)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptors is null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            foreach (var d in descriptors)
            {
                collection.TryAdd(d);
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Scoped"/> service
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        public static void TryAddScoped(
            this IRegistrationCollection collection,
            Type service)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var descriptor = RegistrationDescriptor.Scoped(service, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Scoped"/> service
        /// with the <paramref name="implementationType"/> implementation
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddScoped(
            this IRegistrationCollection collection,
            Type service,
            Type implementationType)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType is null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = RegistrationDescriptor.Scoped(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Scoped"/> service
        /// using the factory specified in <paramref name="implementationFactory"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddScoped(
            this IRegistrationCollection collection,
            Type service,
            Func<IServiceProvider, object> implementationFactory)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            var descriptor = RegistrationDescriptor.Scoped(service, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Scoped"/> service
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        public static void TryAddScoped<TService>(this IRegistrationCollection collection)
            where TService : class
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddScoped(collection, typeof(TService), typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Scoped"/> service
        /// implementation type specified in <typeparamref name="TImplementation"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        public static void TryAddScoped<TService, TImplementation>(this IRegistrationCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddScoped(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Scoped"/> service
        /// using the factory specified in <paramref name="implementationFactory"/>
        /// to the <paramref name="services"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddScoped<TService>(
            this IRegistrationCollection services,
            Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            services.TryAdd(RegistrationDescriptor.Scoped(implementationFactory));
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Singleton"/> service
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        public static void TryAddSingleton(
            this IRegistrationCollection collection,
            Type service)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var descriptor = RegistrationDescriptor.Singleton(service, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Singleton"/> service
        /// with the <paramref name="implementationType"/> implementation
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddSingleton(
            this IRegistrationCollection collection,
            Type service,
            Type implementationType)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType is null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = RegistrationDescriptor.Singleton(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Singleton"/> service
        /// using the factory specified in <paramref name="implementationFactory"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddSingleton(
            this IRegistrationCollection collection,
            Type service,
            Func<IServiceProvider, object> implementationFactory)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            var descriptor = RegistrationDescriptor.Singleton(service, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Singleton"/> service
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        public static void TryAddSingleton<TService>(this IRegistrationCollection collection)
            where TService : class
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddSingleton(collection, typeof(TService), typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Singleton"/> service
        /// implementation type specified in <typeparamref name="TImplementation"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        public static void TryAddSingleton<TService, TImplementation>(this IRegistrationCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddSingleton(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Singleton"/> service
        /// with an instance specified in <paramref name="instance"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="instance">The instance of the service to add.</param>
        public static void TryAddSingleton<TService>(this IRegistrationCollection collection, TService instance)
            where TService : class
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var descriptor = RegistrationDescriptor.Singleton(typeof(TService), instance);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Singleton"/> service
        /// using the factory specified in <paramref name="implementationFactory"/>
        /// to the <paramref name="services"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddSingleton<TService>(
            this IRegistrationCollection services,
            Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            services.TryAdd(RegistrationDescriptor.Singleton(implementationFactory));
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Transient"/> service
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        public static void TryAddTransient(
            this IRegistrationCollection collection,
            Type service)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var descriptor = RegistrationDescriptor.Transient(service, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Transient"/> service
        /// with the <paramref name="implementationType"/> implementation
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddTransient(
            this IRegistrationCollection collection,
            Type service,
            Type implementationType)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType is null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = RegistrationDescriptor.Transient(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="Lifecycle.Transient"/> service
        /// using the factory specified in <paramref name="implementationFactory"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddTransient(
            this IRegistrationCollection collection,
            Type service,
            Func<IServiceProvider, object> implementationFactory)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            var descriptor = RegistrationDescriptor.Transient(service, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Transient"/> service
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        public static void TryAddTransient<TService>(this IRegistrationCollection collection)
            where TService : class
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddTransient(collection, typeof(TService), typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Transient"/> service
        /// implementation type specified in <typeparamref name="TImplementation"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="IRegistrationCollection"/>.</param>
        public static void TryAddTransient<TService, TImplementation>(this IRegistrationCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddTransient(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="Lifecycle.Transient"/> service
        /// using the factory specified in <paramref name="implementationFactory"/>
        /// to the <paramref name="services"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IRegistrationCollection"/>.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddTransient<TService>(
            this IRegistrationCollection services,
            Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            services.TryAdd(RegistrationDescriptor.Transient(implementationFactory));
        }

        ///// <summary>
        ///// Adds a <see cref="RegistrationDescriptor"/> if an existing descriptor with the same
        ///// <see cref="RegistrationDescriptor.ServiceType"/> and an implementation that does not already exist
        ///// in <paramref name="services."/>.
        ///// </summary>
        ///// <param name="services">The <see cref="IRegistrationCollection"/>.</param>
        ///// <param name="descriptor">The <see cref="RegistrationDescriptor"/>.</param>
        ///// <remarks>
        ///// Use <see cref="TryAddEnumerable(IRegistrationCollection, RegistrationDescriptor)"/> when registering a service implementation of a
        ///// service type that
        ///// supports multiple registrations of the same service type. Using
        ///// <see cref="Add(IRegistrationCollection, RegistrationDescriptor)"/> is not idempotent and can add
        ///// duplicate
        ///// <see cref="RegistrationDescriptor"/> instances if called twice. Using
        ///// <see cref="TryAddEnumerable(IRegistrationCollection, RegistrationDescriptor)"/> will prevent registration
        ///// of multiple implementation types.
        ///// </remarks>
        //public static void TryAddEnumerable(
        //    this IRegistrationCollection services,
        //    RegistrationDescriptor descriptor)
        //{
        //    if (services is null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (descriptor is null)
        //    {
        //        throw new ArgumentNullException(nameof(descriptor));
        //    }

        //    var implementationType = descriptor.GetImplementationType();

        //    if (implementationType == typeof(object) ||
        //        implementationType == descriptor.ServiceType)
        //    {
        //        throw new ArgumentException(
        //            Resources.FormatTryAddIndistinguishableTypeToEnumerable(
        //                implementationType,
        //                descriptor.ServiceType),
        //            nameof(descriptor));
        //    }

        //    if (!services.Any(d =>
        //                      d.ServiceType == descriptor.ServiceType &&
        //                      d.GetImplementationType() == implementationType))
        //    {
        //        services.Add(descriptor);
        //    }
        //}

        ///// <summary>
        ///// Adds the specified <see cref="RegistrationDescriptor"/>s if an existing descriptor with the same
        ///// <see cref="RegistrationDescriptor.ServiceType"/> and an implementation that does not already exist
        ///// in <paramref name="services."/>.
        ///// </summary>
        ///// <param name="services">The <see cref="IRegistrationCollection"/>.</param>
        ///// <param name="descriptors">The <see cref="RegistrationDescriptor"/>s.</param>
        ///// <remarks>
        ///// Use <see cref="TryAddEnumerable(IRegistrationCollection, RegistrationDescriptor)"/> when registering a service
        ///// implementation of a service type that
        ///// supports multiple registrations of the same service type. Using
        ///// <see cref="Add(IRegistrationCollection, RegistrationDescriptor)"/> is not idempotent and can add
        ///// duplicate
        ///// <see cref="RegistrationDescriptor"/> instances if called twice. Using
        ///// <see cref="TryAddEnumerable(IRegistrationCollection, RegistrationDescriptor)"/> will prevent registration
        ///// of multiple implementation types.
        ///// </remarks>
        //public static void TryAddEnumerable(
        //    this IRegistrationCollection services,
        //    IEnumerable<RegistrationDescriptor> descriptors)
        //{
        //    if (services is null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (descriptors is null)
        //    {
        //        throw new ArgumentNullException(nameof(descriptors));
        //    }

        //    foreach (var d in descriptors)
        //    {
        //        services.TryAddEnumerable(d);
        //    }
        //}
    }
}