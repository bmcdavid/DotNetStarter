using DotNetStarter.Abstractions;
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
        public ILocatorRegistry CreateRegistry() => new EpiserverStructuremapLocator(_Container);

        /// <summary>
        /// Intialize
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            // try to ensure the scoped dependency resolver is used by waiting til initcomplete to set it.
            //context.InitComplete += (sender, _) =>
            //{
            //    DependencyResolver.SetResolver(new NullableMvcDependencyResolver(ApplicationContext.Default.Locator));
            //};
        }

        /// <summary>
        /// Uninitialize
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context) { }
    }
}
