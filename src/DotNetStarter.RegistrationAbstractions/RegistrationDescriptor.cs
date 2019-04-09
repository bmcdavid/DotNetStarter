// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/DependencyInjection/blob/master/LICENSE.txt for license information.
// modified by Brad McDavid 2018

using System;
using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Specifies the contract for a collection of registration descriptors.
    /// </summary>
    public interface IRegistrationCollection : IList<RegistrationDescriptor> { }

    /// <summary>
    /// Describes a service with its service type, implementation, and lifetime.
    /// </summary>
    public class RegistrationDescriptor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RegistrationDescriptor"/> with the specified <paramref name="implementationType"/>.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
        /// <param name="implementationType">The <see cref="Type"/> implementing the service.</param>
        /// <param name="lifetime">The <see cref="Lifecycle"/> of the service.</param>
        public RegistrationDescriptor(Type serviceType, Type implementationType, Lifecycle lifetime) : this(serviceType, lifetime)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RegistrationDescriptor"/> with the specified <paramref name="instance"/>
        /// as a <see cref="Lifecycle.Singleton"/>.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
        /// <param name="instance">The instance implementing the service.</param>
        public RegistrationDescriptor(Type serviceType, object instance) : this(serviceType, Lifecycle.Singleton)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            ImplementationInstance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RegistrationDescriptor"/> with the specified <paramref name="factory"/>.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
        /// <param name="factory">A factory used for creating service instances.</param>
        /// <param name="lifetime">The <see cref="Lifecycle"/> of the service.</param>
        public RegistrationDescriptor(Type serviceType, Func<IServiceProvider, object> factory, Lifecycle lifetime) : this(serviceType, lifetime)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        private RegistrationDescriptor(Type serviceType, Lifecycle lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }

        /// <inheritdoc />
        public Lifecycle Lifetime { get; }

        /// <inheritdoc />
        public Type ServiceType { get; }

        /// <inheritdoc />
        public Type ImplementationType { get; }

        /// <inheritdoc />
        public object ImplementationInstance { get; }

        /// <inheritdoc />
        public Func<IServiceProvider, object> ImplementationFactory { get; }
                
        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
        /// and the <see cref="Lifecycle.Transient"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Transient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return Describe<TService, TImplementation>(Lifecycle.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="service"/> and <paramref name="implementationType"/>
        /// and the <see cref="Lifecycle.Transient"/> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Transient(Type service, Type implementationType)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType is null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            return Describe(service, implementationType, Lifecycle.Transient);
        }

#if !NET35
        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
        /// <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Transient"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Transient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(typeof(TService), implementationFactory, Lifecycle.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Transient"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(typeof(TService), implementationFactory, Lifecycle.Transient);
        }
#endif
        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="service"/>, <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Transient"/> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(service, implementationFactory, Lifecycle.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Scoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return Describe<TService, TImplementation>(Lifecycle.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="service"/> and <paramref name="implementationType"/>
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Scoped(Type service, Type implementationType)
        {
            return Describe(service, implementationType, Lifecycle.Scoped);
        }

#if !NET35
        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
        /// <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Scoped<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(typeof(TService), implementationFactory, Lifecycle.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Scoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(typeof(TService), implementationFactory, Lifecycle.Scoped);
        }
#endif

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="service"/>, <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Scoped(Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(service, implementationFactory, Lifecycle.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
        /// and the <see cref="Lifecycle.Singleton"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return Describe<TService, TImplementation>(Lifecycle.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="service"/> and <paramref name="implementationType"/>
        /// and the <see cref="Lifecycle.Singleton"/> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton(Type service, Type implementationType)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType is null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            return Describe(service, implementationType, Lifecycle.Singleton);
        }

#if !NET35
        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
        /// <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Singleton"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(typeof(TService), implementationFactory, Lifecycle.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Singleton"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(typeof(TService), implementationFactory, Lifecycle.Singleton);
        }
#endif

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="serviceType"/>, <paramref name="implementationFactory"/>,
        /// and the <see cref="Lifecycle.Singleton"/> lifetime.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton(
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory is null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return Describe(serviceType, implementationFactory, Lifecycle.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <typeparamref name="TService"/>, <paramref name="implementationInstance"/>,
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationInstance">The instance of the implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton<TService>(TService implementationInstance)
            where TService : class
        {
            if (implementationInstance is null)
            {
                throw new ArgumentNullException(nameof(implementationInstance));
            }

            return Singleton(typeof(TService), implementationInstance);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="serviceType"/>, <paramref name="implementationInstance"/>,
        /// and the <see cref="Lifecycle.Scoped"/> lifetime.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationInstance">The instance of the implementation.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Singleton(
            Type serviceType,
            object implementationInstance)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationInstance is null)
            {
                throw new ArgumentNullException(nameof(implementationInstance));
            }

            return new RegistrationDescriptor(serviceType, implementationInstance);
        }

        private static RegistrationDescriptor Describe<TService, TImplementation>(Lifecycle lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            return Describe(
                typeof(TService),
                typeof(TImplementation),
                lifetime: lifetime);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="serviceType"/>, <paramref name="implementationType"/>,
        /// and <paramref name="lifetime"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Describe(Type serviceType, Type implementationType, Lifecycle lifetime)
        {
            return new RegistrationDescriptor(serviceType, implementationType, lifetime);
        }

        /// <summary>
        /// Creates an instance of <see cref="RegistrationDescriptor"/> with the specified
        /// <paramref name="serviceType"/>, <paramref name="implementationFactory"/>,
        /// and <paramref name="lifetime"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A new instance of <see cref="RegistrationDescriptor"/>.</returns>
        public static RegistrationDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, Lifecycle lifetime)
        {
            return new RegistrationDescriptor(serviceType, implementationFactory, lifetime);
        }
    }
}