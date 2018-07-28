---
title: DotNetStarter - Customizing the startup process
---
# DotNetStarter - Customizing the startup process

The startup process is owned by the application owner in which DotNetStarter is ran. In order to run efficiently, the application should be fine-tuned to only scan assemblies utilizing DotNetStarter.Abstractions or DotNetStarter.RegistrationAbsractions. Below is an exmple startup using the StartupBuilder configuration class

The StartupBuilder provides a fluent API to change almost every aspect of the startup process, but can also be as simple as:

```cs
DotNetStarter.Configure.StartupBuilder.Create().Run();
```
Or more finely-tuned for assembly scanning and overriding defaults.

```cs
var builder = DotNetStarter.Configure.StartupBuilder.Create();
builder
    // configure the assemblies to scan
    .ConfigureAssemblies(assemblies =>
    {
        assemblies
            // Filters assemblies for ones using the [assembly: DotNetStarter.Abstractions.DiscoverableAssembly] 
            .WithDiscoverableAssemblies() // for ASP.NET Core projects an initial list of assemblies must be provided
            .WithAssemblyFromType<RegistrationConfiguration>()
            .WithAssembliesFromTypes(typeof(StartupBuilder), typeof(BadStartupModule));
    })
    .ConfigureStartupModules(modules =>
    {
        modules
            // ability to manually add ILocatorConfigure modules after the scanned ones
            .ConfigureLocatorModuleCollection(configureModules =>
            {
                configureModules.Add(sut);
            })
            // ability to manually add IStartupModule modules after the scanned ones
           .ConfigureStartupModuleCollection(collection =>
            {
                collection.AddType<TestStartupModule>();
            })
            // if there are any modules that are acting badly or if you want to customize remove some to insert customized versions.
            .RemoveStartupModule<BadStartupModule>()
            .RemoveConfigureModule<BadConfigureModule>();
    })
    // ability to customize environment object, which can be used make registration decisions based on environment
    .UseEnvironment(new StartupEnvironment("UnitTest1", ""))
    // override default objects
    .OverrideDefaults(defaults =>
    {
        defaults
            .UseLogger(new StringLogger(LogLevel.Info));
    })
    .Build() // configures the ILocator
    .Run() // Runs IStartupModule registrations;
```