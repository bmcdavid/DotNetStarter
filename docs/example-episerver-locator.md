---
title: DotNetStarter - Episerver locator and Depencency Resolver
---
# DotNetStarter - Episerver locator and Depencency Resolver

Below is an example of how to wireup with Episerver's structuremap configured container.

## Required NuGet packages

* DotNetStarter
* DotNetStarter.Extensions.Mvc - for MVC depencency resolving
* DotNetStarter.Structuremap - for Epi 11
* or DotNetStarter.Locators.StructureMapSigned - for Epi 9 or 10

```cs
using DotNetStarter;
using DotNetStarter.Abstractions;
using DotNetStarter.Configure;
using DotNetStarter.Web;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System.Collections;
using System.Configuration;

namespace Example.Business.Initialization
{
    /// <summary>
    /// Episerver initalization module to hook in DotNetStarter into the startup process
    /// </summary>
    [ModuleDependency]
    public class WireupDotNetStarter : IConfigurableModule
    {
        private ILocatorRegistry _registry;
        private StartupBuilder _startupBuilder;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            // for Episerver 9/10 use DotNetStarter.Locators.StructuremapSigned package
            // for Episerver 11 use DotNetStarter.Structuremap package
            var registryFactory = new DotNetStarter.Locators.StructureMapFactory(context.StructureMap());
            _startupBuilder = StartupBuilder
                .Create()
                // create an environment, values are typically Local, Development, Staging or Production
                // can be used to make conditional registrations in ILocatorConfigure modules
                .UseEnvironment(new StartupEnvironmentWeb(ConfigurationManager.AppSettings["DotNetStarter.Environment"]))
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithDiscoverableAssemblies()
                        .WithAssemblyFromType<WireupDotNetStarter>() // this project, for controllers, services, etc
                        ;
                })
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLocatorRegistryFactory(registryFactory)
                        .UseLogger(new StringLogger(LogLevel.Error, 1024000));
                })
                .Build();
        }

        // run the IStartupModule instances here to avoid any resolving too early in Episerver process
        public void Initialize(InitializationEngine context) => _startupBuilder.Run();

        public void Uninitialize(InitializationEngine context) { }
    }
}
```