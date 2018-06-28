# DotNetStarter Read Me

[![Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter)

The goal of this project is to create a startup and IoC/DI framework for netframeworks 3.5+ and netstandard 1.0+ which allows packages to support container registrations without a specific container defined.

Package  | Version 
-------- | :------------ 
[DotNetStarter](https://www.nuget.org/packages/DotNetStarter/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.svg)](https://badge.fury.io/nu/DotNetStarter)
[DotNetStarter.Abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Abstractions.svg)](https://badge.fury.io/nu/DotNetStarter.Abstractions)
[DotNetStarter.RegistrationAbstractions](https://www.nuget.org/packages/DotNetStarter.RegistrationAbstractions/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.RegistrationAbstractions.svg)](https://badge.fury.io/nu/DotNetStarter.RegistrationAbstractions)
[DotNetStarter.Web](https://www.nuget.org/packages/DotNetStarter.Web/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Web.svg)](https://badge.fury.io/nu/DotNetStarter.Web)
[DotNetStarter.Owin](https://www.nuget.org/packages/DotNetStarter.Owin/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Owin.svg)](https://badge.fury.io/nu/DotNetStarter.Owin)
[DotNetStarter.DryIoc](https://www.nuget.org/packages/DotNetStarter.DryIoc/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.DryIoc.svg)](https://badge.fury.io/nu/DotNetStarter.DryIoc)
[DotNetStarter.Structuremap](https://www.nuget.org/packages/DotNetStarter.Structuremap/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Structuremap.svg)](https://badge.fury.io/nu/DotNetStarter.Structuremap)
[DotNetStarter.Extensions.Mvc](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Extensions.Mvc.svg)](https://badge.fury.io/nu/DotNetStarter.Extensions.Mvc)
[DotNetStarter.Extensions.WebApi](https://www.nuget.org/packages/DotNetStarter.Extensions.WebApi/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Extensions.WebApi.svg)](https://badge.fury.io/nu/DotNetStarter.Extensions.WebApi)

## Getting Started

* [**Important:** Breaking Changes](https://bmcdavid.github.io/DotNetStarter/breaking-changes.html)
* [Full Documentation](https://bmcdavid.github.io/DotNetStarter/)

To kickoff the startup modules and configure the locator please execute the following early in the project startup, for example global.asax class constructor for ASP.Net Framework web applications.

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

### Inversion of Control / Dependency Injection
An IoC/DI package must be installed to enable the ILocator, two are provided by default DotNetStarter.DryIoc and DotNetStarter.Structuremap.
They can also be swapped at runtime via the assembly attribute as noted below for DryIoc:

```cs
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.DryIocLocatorFactory))]
```

## Known Issues

* IStartupModule.Shutdown doesn't execute in netcoreapps. Workaround is attach to unloading event using an IStartup module to resolve the IShutdownHandler as noted below:
```cs
[StartupModule(typeof(RegistrationConfiguration))]
public class ShutdownHook : IStartupModule
{
    IShutdownHandler _ShutdownHandler;

    void IStartupModule.Shutdown()
    {
        System.Runtime.Loader.AssemblyLoadContext.Default.Unloading -= Default_Unloading;
    }

    void IStartupModule.Startup(IStartupEngine engine)
    {
        _ShutdownHandler = engine.Locator.Get<IShutdownHandler>(); // cannot inject it, to avoid recursion
        System.Runtime.Loader.AssemblyLoadContext.Default.Unloading -= Default_Unloading;
        System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += Default_Unloading;
    }

    void Default_Unloading(System.Runtime.Loader.AssemblyLoadContext obj)
    {
        _ShutdownHandler.Shutdown();
    }
}
```

* netcoreapps require custom assembly loading noted below:
```cs
// Add the following lines in the Startup class constructor, for netcore assembly loading
Func<IEnumerable<Assembly>> assemblyLoader = () =>
{
    var runtimeId = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
    var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames(Microsoft.Extensions.DependencyModel.DependencyContext.Default, runtimeId);

    return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
};

DotNetStarter.ApplicationContext.Startup(assemblies: assemblyLoader());
```
## Examples of DI/IOC, requires an ILocator package such as DotNetStarter.DryIoc or DotNetStarter.StructureMap
### Registration
```cs
public interface ITest
{
    string SayHi(string n);
}

[Registration(typeof(ITest), Lifecycle.Transient)]
public class Test : ITest
{
    public string SayHi(string n) => "Hello " + n;
}
```
### Usage
```cs
// Import<T> is a struct wrapper for DotNetStarter.ApplicationContext.Default.Locator and can be used when scoping isn't required.
// Also, Import<T> should only be used when Construction Injection is not available.
public Import<ITest> TestService { get; set; }

public void ExampleMethod()
{
    string message = TestService.Service.SayHi("User");
}
```