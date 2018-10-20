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
            if (serviceType == null)
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
            if (serviceType == null)
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
            if (serviceType == null)
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
    }
}