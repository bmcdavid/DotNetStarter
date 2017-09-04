﻿using DotNetStarter.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using StructureMap;
using System;

// instructs DotNetStarter to use this to create ILocatorRegistry
[assembly: LocatorRegistryFactory(typeof(DotNetStarter.Extensions.Episerver.EpiserverLocatorSetup))]

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Creates a DotNetStarter ILocatorFactory using Episerver's structuremap instance
    /// </summary>
    [ModuleDependency]
    public class EpiserverLocatorSetup : IConfigurableModule, ILocatorRegistryFactory
    {
        static IContainer _Container; // must be static to share between instances

        /// <summary>
        /// Invokable action to startup DotNetStarter when the Episerver container is set.
        /// </summary>
        public static Action<IContainer> ContainerSet = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
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
            if (_Container == null)
            {
                throw new NullReferenceException($"{typeof(ApplicationContext).FullName}.{nameof(ApplicationContext.Startup)}" +
                    $" was invoked before Episerver initialization. Please assign an action to {typeof(EpiserverLocatorSetup).FullName}.{nameof(ContainerSet)}" +
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
            context.InitComplete += (sender, _) =>
            {
                ApplicationContext.Startup(); // defaut startup call, but can be changed with ContainerSet action
            };
        }

        /// <summary>
        /// Uninitialize
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context) { }
    }
}
