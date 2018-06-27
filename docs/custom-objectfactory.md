---
title: DotNetStarter - Customizing the startup process
---
# DotNetStarter - Customizing the startup process

The startup process is owned by the application owner in which DotNetStarter is ran. In order to run efficiently, the application should fine tune this process to only scan assemblies using DotNetStarter.Abstractions or DotNetStarter.RegistrationAbsractions. Below is an exmple startup using the StartupBuilder configuration class

```cs
var builder = DotNetStarter.Configure.StartupBuilder.Create();
builder
	// configure the assemblies to scan
    .ConfigureAssemblies(assemblies =>
    {
        assemblies
            .WithDiscoverableAssemblies() // for ASP.NET Framework projects only
            .WithAssemblyFromType<RegistrationConfiguration>()
            .WithAssembliesFromTypes(typeof(StartupBuilder), typeof(BadStartupModule));
    })
    .ConfigureStartupModules(modules =>
    {
		// if there are any modules that are acting badly or if you want to customize versions.
        modules
            .RemoveStartupModule<BadStartupModule>()
            .RemoveConfigureModule<BadConfigureModule>();
    })
    // customize environment object
    .UseEnvironment(new StartupEnvironment("UnitTest1", ""))
    // override default objects
	.OverrideDefaults(defaults =>
    {
        defaults
            .UseLocatorRegistryFactory(new StructureMapFactory())
            .UseLogger(new StringLogger(LogLevel.Info));
    })
    .Build() // configures the ILocator
    .Run() // Runs IStartupModule registrations;
```

The StartupBuilder provides a fluent API to change almost every aspect of the startup process.

## Custom IStartupObjectFactory (older and will be deprecated)

The object factory is responsible for creating objects before the Ioc/DI container is setup. 
Implementations must use the IStartupObjectFactory interface.

 ## Startup Assignment
 To create a custom object factory, an implementation of IStartupObjectFactory can be passed to **DotNetStarter.ApplicationContext.Startup()**