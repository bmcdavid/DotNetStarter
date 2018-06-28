# DotNetStarter.StructureMapSigned Read Me

This package should be installed on Episerver sites and is enabled using an Episerver init module, example below:

```cs
using DotNetStarter.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

// instructs DotNetStarter to use this to create ILocatorRegistry
[assembly: LocatorRegistryFactory(typeof(ExampleNamespace.Business.Initialization.WireupDotNetStarter),
    typeof(DotNetStarter.Locators.StructureMapSignedFactory))]

namespace ExampleNamespace.Business.Initialization
{
    /// <summary>
    /// Episerver initalization module to hook in DotNetStarter into the startup process
    /// </summary>
    [ModuleDependency] // important to have no dependencies
    public class WireupDotNetStarter : IConfigurableModule, ILocatorRegistryFactory
    {
        private static ILocatorRegistry _Registry; // static so DotNetStarter can retrieve it

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _Registry = new DotNetStarter.StructureMapSignedLocator(context.StructureMap());

            var scannableAssemblies = DotNetStarter.ApplicationContext.GetScannableAssemblies();
            var additionalAssemblies = new Assembly[]
            {
                typeof(WireupDotNetStarter).Assembly, // types in this assembly
            };

            var startupConfiguration = new DotNetStarter.StartupConfiguration
            (
                scannableAssemblies.Union(additionalAssemblies),
                null, // no assembly filter, as we prefiltered above
                new DotNetStarter.AssemblyScanner(),
                new DotNetStarter.DependencyFinder(),
                new DotNetStarter.DependencySorter(CreateDependencyNode), // pass delegate to create dependency nodes
                new DotNetStarter.StringLogger(LogLevel.Error), // logs to a string builder
                new DotNetStarter.StartupModuleFilter(), // can be customized to remove startup modules,
                new DotNetStarter.TimedTaskManager(CreateRequestSettingsProvider), // pass delegate to create request provider settings
                new DotNetStarter.StartupEnvironment
                (
                    // example values should be: Production, Development, Staging, Local, Testing
                    ConfigurationManager.AppSettings["ApplicationEnvironment"], // can be assigned from anything, but simplest solution is app setting
                    AppDomain.CurrentDomain.BaseDirectory
                )
            );

            // startup with our custom startup configuration
            DotNetStarter.ApplicationContext.Startup(configuration: startupConfiguration);
        }

        public ILocatorRegistry CreateRegistry() => _Registry;

        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }

        IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return new DotNetStarter.DependencyNode(nodeType, attributeType);
        }

        IRequestSettingsProvider CreateRequestSettingsProvider()
        {
            return new DotNetStarter.RequestSettingsProvider();
        }
    }
}
```