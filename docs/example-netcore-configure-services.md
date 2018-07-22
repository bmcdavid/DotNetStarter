---
title: DotNetStarter - ASP.Net Core Configure Services
---
# DotNetStarter - ASP.Net Core Configure Services

Below are examples of how to configure the IServiceProvider in Configure Services for the supported locators.

## DotNetStarter

DotNetStarter can now create the service provider in the configure set. The only issue with using it is it selects the greediest constructor and not all services are wired up in that manner. Below is a simple example that works with all three of the maintained locators.

### Required Packages

* DotNetStarter.RegistrationAbstractions
* DotNetStarter.Abstractions
* DotNetStarter
* One of the following:
  * DotNetStarter.DryIoc
  * DotNetStarter.Structuremap
  * DotNetStarter.Locators.LightInject

```cs
public class Startup
{
    private readonly IHostingEnvironment _env;
    private DotNetStarter.Configure.StartupBuilder startupBuilder;

    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        startupBuilder.Run(); // to run all IStartupModules

        // omitted for brevity
    }

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

        // begin DotNetStarter with DryIoc Container
        startupBuilder = DotNetStarter.Configure.StartupBuilder.Create()
            .UseEnvironment(new StartupEnvironment(_env.EnvironmentName, _env.ContentRootPath))
            .ConfigureAssemblies(assemblies =>
            {
                assemblies
                .WithDiscoverableAssemblies(LoadAssemblies())
                .WithAssemblyFromType<Startup>();
            })
            // provide an ILocator registry factory based on one of the support ILocator packages
            .OverrideDefaults(d => d.UseLocatorRegistryFactory(CreateRegistryFactory(services)))
            .Build(useApplicationContext: false);

        return startupBuilder.StartupContext.Locator.Get<IServiceProvider>();
    }

    // for loading all assemblies
    private ICollection<Assembly> LoadAssemblies()
    {
        var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames
        (
            Microsoft.Extensions.DependencyModel.DependencyContext.Default,
            Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier()
        );

        return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name))).ToList();
    }

    private ILocatorRegistryFactory CreateRegistryFactory(IServiceCollection services)
    {
        // return an ILocatorRegistryFactory as noted below in the examples
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
/// <summary>
/// configures a DryIoc locator with services provided by IServiceCollection
/// </summary>
/// <param name="services"></param>
/// <returns></returns>
private ILocatorRegistryFactory CreateDryIocFactory(IServiceCollection services)
{
    // default container Rules for dotnet core
    var rules = DryIoc.Rules.Default
        .With(DryIoc.FactoryMethod.ConstructorWithResolvableArguments)
        .WithFactorySelector(DryIoc.Rules.SelectLastRegisteredFactory())
        .WithTrackingDisposableTransients(); //used in transient delegate cases

    var container = new DryIoc.Container(rules);
    DryIoc.Microsoft.DependencyInjection.DryIocAdapter.Populate(container, services); // configures DryIoc with IServiceCollection

    return new DotNetStarter.Locators.DryIocLocatorFactory(container);
}
```

## Structuremap Locator

### Required Nuget Packages
* DotNetStarter
* DotNetStarter.Structuremap
* Structuremap.Microsoft.DependencyInjection

Replace the **CreateRegistryFactory** call in the above example with the below:

```cs
/// <summary>
/// configures a Structuremap locator with services provided by IServiceCollection
/// </summary>
/// <param name="services"></param>
/// <returns></returns>
private ILocatorRegistryFactory CreateStructuremapFactory(IServiceCollection services)
{
    var container = new StructureMap.Container();
    container.Configure(config =>
    {
        StructureMap.ContainerExtensions.Populate(config,services); // add services
    });

    return new StructureMapFactory(container);
}
```