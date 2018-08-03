using Autofac;
using Autofac.Builder;
using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Creates Autofac registry and locator
    /// </summary>
    public class AutofacLocatorRegistryFactory : ILocatorRegistryFactory
    {
        private readonly ContainerBuilder _containerBuilder;
        private readonly ContainerBuildOptions? _containerBuilderOptions;
        private readonly Func<IContainer> _containerFactory;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AutofacLocatorRegistryFactory() => _containerBuilder = new ContainerBuilder();

        /// <summary>
        /// Injected Constructor where DotNetStarter creates the container
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="options"></param>
        public AutofacLocatorRegistryFactory(ContainerBuilder containerBuilder, ContainerBuildOptions options)
        {
            _containerBuilder = containerBuilder;
            _containerBuilderOptions = options;
        }

        /// <summary>
        /// Injected Constructor where DotNetStarter creates the container
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="containerFactory"></param>
        public AutofacLocatorRegistryFactory(ContainerBuilder containerBuilder, Func<IContainer> containerFactory)
        {
            _containerBuilder = containerBuilder;
            _containerFactory = containerFactory;
        }

        /// <summary>
        /// Creates a locator
        /// </summary>
        /// <returns></returns>
        public ILocator CreateLocator() => _containerFactory != null ?
            new AutofacLocator(_containerFactory) :
            new AutofacLocator(_containerBuilder, _containerBuilderOptions ?? ContainerBuildOptions.None);

        /// <summary>
        /// Creates a service registry using acontainer builder
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new AutofacLocatoryRegistry(_containerBuilder);
    }
}