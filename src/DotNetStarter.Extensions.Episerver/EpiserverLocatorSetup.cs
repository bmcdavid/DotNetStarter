﻿using DotNetStarter.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System;

// instructs DotNetStarter to use this to create ILocatorRegistry
[assembly: LocatorRegistryFactory(typeof(DotNetStarter.Extensions.Episerver.EpiserverLocatorSetup))]

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Creates a DotNetStarter ILocatorFactory using Episerver's structuremap instance
    /// </summary>
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class EpiserverLocatorSetup : IConfigurableModule, ILocatorRegistryFactory
    {
        static StructureMap.IContainer _Container; // must be static to share between instances

        static ILocatorRegistry _LocatorRegistry;

        /// <summary>
        /// Invokable action to startup DotNetStarter when the Episerver container is set. Use a System.Web.PreApplicationStartMethod to assign a startup action;
        /// </summary>
        [Obsolete]
        public static Action<StructureMap.IContainer> ContainerSet = null;

        /// <summary>
        /// Invoked to create a locator registry since Structuremap is now modular in Episerver. Use a System.Web.PreApplicationStartMethod to assign a startup func;
        /// </summary>
        public static Func<ServiceConfigurationContext, ILocatorRegistry> CreateLocatorRegistry = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _LocatorRegistry = CreateLocatorRegistry?.Invoke(context);

            // todo: remove lines below in Epi v11
            var container = context.Container;
            _Container = container; // store the containr for use in CreateRegistry
            ContainerSet?.Invoke(container);
        }

        /// <summary>
        /// Create locator registry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry()
        {
            if (_LocatorRegistry != null)
                return _LocatorRegistry;

            // todo: remove lines below in Epi v11

            if (_Container == null)
            {
                throw new NullReferenceException($"{typeof(ApplicationContext).FullName}.{nameof(ApplicationContext.Startup)}" +
                    $" was invoked before Episerver initialization. Please assign an action to {typeof(EpiserverLocatorSetup).FullName}.ContainerSet" +
                    " to invoke startup when the container reference is set in the global.asax class constructor.");
            }

            return new EpiserverStructuremapLocator(_Container);
        }

        /// <summary>
        /// Intialize
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            // ensure DotNetStarter has started
            context.InitComplete += (sender, args) =>
            {
                // ensures DotNetStarter has started
                var locator = ApplicationContext.Default.Locator;
            };
        }

        /// <summary>
        /// Uninitialize
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context) { }
    }
}
