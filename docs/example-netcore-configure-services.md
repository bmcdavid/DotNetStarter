---
title: DotNetStarter - ASP.Net Core Configure Services
---
# DotNetStarter - ASP.Net Core Configure Services

Below are examples of how to configure the IServiceProvider in Configure Services for some of supported locators. By default, DotNetStarter will use the greediest constructor, which services in the service collection do not always use. To resolve this, use a DI container's adapter package for wiring up the service collection, then pass the already configured container instance to an ILocatorRegistryFactory provided by a DotNetStarter supported locator. 

### Required Packages

* DotNetStarter.RegistrationAbstractions
* DotNetStarter.Abstractions
* DotNetStarter
* One of the following:
  * DotNetStarter.DryIoc
  * DotNetStarter.Structuremap
  * DotNetStarter.Locators.LightInject

```cs
using System;
using DotNetStarter.Abstractions;
using DotNetStarter.Locators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleStartup
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly DotNetStarter.Configure.StartupBuilder _startupBuilder;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnv)
        {
            Configuration = configuration;
            _hostingEnv = hostingEnv;
            _startupBuilder = DotNetStarter.Configure.StartupBuilder.Create();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // service configuration
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // configures dotnetstarter with service collection
            return ConfigureDotNetStarter(services);
        }

        private IServiceProvider ConfigureDotNetStarter(IServiceCollection services)
        {
            _startupBuilder
                //environment allows for conditional check to swap services, perform tasks only in production, etc
                .UseEnvironment(new DotNetStarter.StartupEnvironment(_hostingEnv.EnvironmentName, _hostingEnv.ContentRootPath))
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        //scan all types with [assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
                        .WithDiscoverableAssemblies()
                        //scan types in this project
                        .WithAssemblyFromType<Startup>();
                })
                // provide an ILocator registry factory based on one of the support ILocator packages
                // listed at https://bmcdavid.github.io/DotNetStarter/locators.html
                .OverrideDefaults(d => d.UseLocatorRegistryFactory(CreateRegistryFactory(services)))
                .Build(useApplicationContext: false) // executes all ILocatorConfigure instances
                .Run(); // executes all IStartupModule instances

            return _startupBuilder.StartupContext.Locator.Get<IServiceProvider>();
        }    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
             //omitted for brevity
        }
    }
}
```

## DryIoc Locator

### Required Nuget packages

* DotNetStarter
* DotNetStarter.DryIoC
* DryIoc.Microsoft.DependencyInjection

Replace the **CreateRegistryFactory** call in the above example with the below:

```cs
// uses DotNetStarter.DryIoc package
private static ILocatorRegistryFactory CreateDryIocRegistryFactory(IServiceCollection services)
{
    // create default container Rules for dotnet core
    var rules = DryIoc.Rules.Default
        .With(DryIoc.FactoryMethod.ConstructorWithResolvableArguments)
        .WithFactorySelector(DryIoc.Rules.SelectLastRegisteredFactory())
        .WithTrackingDisposableTransients();

    var container = new DryIoc.Container(rules);
    // configures DryIoc with IServiceCollection using DryIoc.Microsoft.DependencyInjection
    DryIoc.Microsoft.DependencyInjection.DryIocAdapter.Populate(container, services);

    return new DryIocLocatorFactory(container);
}    
```

## Structuremap Locator

### Required Nuget Packages
* DotNetStarter
* DotNetStarter.Structuremap
* Structuremap.Microsoft.DependencyInjection

Replace the **CreateRegistryFactory** call in the above example with the below:

```cs
// uses DotNetStarter.Structuremap package
private static ILocatorRegistryFactory CreateStructuremapRegistryFactory(IServiceCollection services)
{
    var container = new StructureMap.Container();
    container.Configure(config =>
    {
        // maps services using Structuremap.Microsoft.DependencyInjection
        StructureMap.ContainerExtensions.Populate(config, services);
    });

    return new StructureMapFactory(container);
}
```
