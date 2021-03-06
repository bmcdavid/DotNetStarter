[![Master Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de/branch/master?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter/branch/master)

DotNetStarter is a framework for composing applications where many components are provided by NuGet packages. The main audiences of DotNetStarter are package authors and application developers.

Package authors can depend on either the [configuration and startup abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) or the [registration attribute abstractions](https://www.nuget.org/packages/DotNetStarter.RegistrationAbstractions/) to create their components. The components can then be designed with constructor dependency injection in mind. These classes can then be registered by using the [RegistrationAttribute](https://bmcdavid.github.io/DotNetStarter/register.html) or in a startup module implementing [ILocatorConfigure](https://bmcdavid.github.io/DotNetStarter/register.html). Packages may also perform tasks during startup and shutdown using the [IStartupModule](https://bmcdavid.github.io/DotNetStarter/modules.html) interface.

Application developers can install the DotNetStarter package, a [locator](https://bmcdavid.github.io/DotNetStarter/locators.html) (container wrapper) package, any extension such as [MVC](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) for the full .NET framework, and any NuGet packages utilizing the abstractions. Developers have full control over the [startup process](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html) which can be customized through code configuration at almost every level using a fluent configuration API. The framework also supports a wide variety of .NET frameworks from .NET version 3.5 and up, as well as the [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) starting at 1.0.

## Getting Started

* [**Important:** Breaking Changes](https://bmcdavid.github.io/DotNetStarter/breaking-changes.html)
* [Full Documentation](https://bmcdavid.github.io/DotNetStarter/)
* [Known Issues](https://bmcdavid.github.io/DotNetStarter/known-issues.html)

To kickoff the startup modules and configure the locator please execute the following early in the project startup, for example global.asax class constructor for .NET Framework web applications.

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
            .WithDiscoverableAssemblies()
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

## Inversion of Control / Dependency Injection
An IoC/DI package must be installed to enable the ILocator. There are several provided by default:

* [DotNetStarter.DryIoc](https://www.nuget.org/packages/DotNetStarter.DryIoc/)
* [DotNetStarter.LightInject](https://www.nuget.org/packages/DotNetStarter.Locators.LightInject/)
* [DotNetStarter.Structuremap](https://www.nuget.org/packages/DotNetStarter.StructureMap/)

They can also be swapped at compile time by the application developer by overriding the default:

```cs
DotNetStarter.Configure.StartupBuilder.Create()
    .ConfigureAssemblies(a => a.WithDiscoverableAssemblies())
    .OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.DryIocLocatorFactory())) //uses DryIoc
    .Build()
    .Run();
```
If the default isn't overridden a locator is discovered at runtime in the configured assemblies by an assembly attribute as noted below for DryIoc.
```cs
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.DryIocLocatorFactory))]
```

## Examples of DI/IOC
### Service Registration
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
Constructor injection is the preferred method of consuming services as shown below.
```cs
[Registration(typeof(TestService), Lifecycle.Transient)]
public class TestService
{
    private readonly ITest _test;
    
    public TestService(ITest test)
    {
        _test = test;
    }

    public void ExampleMethod()
    {
        string message = _test.SayHi("User");
    }
}
```
Cases may exist where constructor injection is not available, in these cases an Import&lt;T> could be used to resolve services. A best practice when using Import &lt;T> is to make the dependency known by using a public property, which may be overridden using Import<T>.Accessor by any consumers.

```cs
public class TestService
{
    public Import<ITest> TestImple { get; set; }

    public void ExampleMethod()
    {
        string message = TestImple.Service.SayHi("User");
    }
}

```

## Abstractions

All attributes, baseclasses and interfaces reside in the DotNetStarter.Abstractions namespace. Documentation is provided in the intellisense.