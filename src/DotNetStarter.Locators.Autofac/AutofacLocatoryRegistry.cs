using Autofac;
using Autofac.Builder;
using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Autofac locator registry
    /// </summary>
    public class AutofacLocatoryRegistry : ILocatorRegistry, ILocatorRegistryWithResolveConfigureModules
    {
        private ContainerBuilder _containerBuilder;
        private List<Configurations> _configure = new List<Configurations>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="containerBuilder"></param>
        public AutofacLocatoryRegistry(ContainerBuilder containerBuilder) => _containerBuilder = containerBuilder;

        /// <summary>
        /// ContainerBuilder instance
        /// </summary>
        public object InternalContainer => _containerBuilder;

        /// <summary>
        /// Adds service implementation for service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifecycle = Lifecycle.Transient)
        {
            ConfirmService(serviceType, serviceImplementation);
            if (serviceType == typeof(ILocatorConfigure))
            {
                _configure.Add(new Configurations { ConfigureType = serviceImplementation });
            }

            if (serviceType.IsGenericType())
            {
                _containerBuilder
                    .RegisterGeneric(serviceImplementation)
                    .As(serviceType)
                    .ConfigureLifecycle(lifecycle, null);
            }
            else
            {
                _containerBuilder
                    .RegisterType(serviceImplementation)
                    .As(serviceType)
                    .ConfigureLifecycle(lifecycle, null);
            }
        }

        /// <summary>
        /// Adds delegate creator for service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="lifecycle"></param>
        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifecycle)
        {
            var registration = RegistrationBuilder.ForDelegate(serviceType, (context, parameters) =>
            {
                return implementationFactory(context.Resolve<ILocatorAmbient>().Current);
            })
            .ConfigureLifecycle(lifecycle, null)
            .CreateRegistration();

            _containerBuilder.RegisterComponent(registration);
        }

        /// <summary>
        /// Adds service instance for type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void Add(Type serviceType, object serviceInstance)
        {
            if (serviceType == typeof(ILocatorConfigure))
            {
                _configure.Add(new Configurations { ConfigureInstance = serviceInstance as ILocatorConfigure });
            }

            _containerBuilder
                .RegisterInstance(serviceInstance)
                .As(serviceType).SingleInstance();
        }

        /// <summary>
        /// Adds service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        public void Add<TService, TImpl>(string key = null, Lifecycle lifecycle = Lifecycle.Transient) where TImpl : TService => Add(typeof(TService), typeof(TImpl), lifecycle: lifecycle);

        IEnumerable<ILocatorConfigure> ILocatorRegistryWithResolveConfigureModules.ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration startupConfiguration)
        {
            foreach (var module in _configure)
            {
                if (module.ConfigureInstance != null)
                    yield return (ILocatorConfigure)module.ConfigureInstance;
                else
                    yield return (ILocatorConfigure)Activator.CreateInstance(module.ConfigureType);
            }
        }

        private static void ConfirmService(Type serviceType, Type serviceImplementation)
        {
            if (!serviceType.IsAssignableFromCheck(serviceImplementation))
            {
                if (!serviceType.IsGenericType())
                {
                    ThrowRegisterException(serviceType, serviceImplementation);
                }
                else
                {
                    if (!serviceImplementation.IsGenericInterface(serviceType))
                    {
                        ThrowRegisterException(serviceType, serviceImplementation);
                    }
                }
            }
        }

        private static void ThrowRegisterException(Type service, Type implementation)
        {
            var ex = new ArgumentException($"{implementation.FullName} cannot be converted to {service.FullName}!");

            throw ex;
        }
    }
}