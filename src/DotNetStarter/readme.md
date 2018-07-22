# DotNetStarter Read Me

DotNetStarter is a framework for composing applications where many components are provided by NuGet packages. There are two main audiences: package authors and application owners.

Package authors can depend on either the configuration and startup abstractions or the registration attribute abstractions to create their components. The components can then be designed with constructor dependency injection in mind. These classes can then be registered by using the [RegistrationAttribute](https://bmcdavid.github.io/DotNetStarter/register.html) or in a startup module implementing [ILocatorConfigure](https://bmcdavid.github.io/DotNetStarter/register.html). Packages may also perform tasks during startup and shutdown using the [IStartupModule](https://bmcdavid.github.io/DotNetStarter/modules.html) interface.

Application owners can install the DotNetStarter package, a locator (container wrapper) package, any extension such as MVC for the full ASP.Net framework, and any NuGet packages utilizing the abstractions. Owners have full control over the [startup process](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html) which can be customized through code configuration at almost every level using a fluent configuration API. The framework also supports a wide variety of .NET frameworks, specifically ASP.NET version 3.5+ and the [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) starting at 1.0.


* [**Important:** Breaking Changes](https://bmcdavid.github.io/DotNetStarter/breaking-changes.html)

## Known Issues

* IStartupModule.Shutdown doesn't execute in netcoreapps. Workaround is to add an init module and attach to unloading event as noted below:
```
[StartupModule]
public class ShutdownHook : IStartupModule
{
    public void Shutdown(IStartupEngine engine)
    {
        AssemblyLoadContext.Default.Unloading -= Default_Unloading;
    }

    public void Startup(IStartupEngine engine)
    {
        AssemblyLoadContext.Default.Unloading -= Default_Unloading;
        AssemblyLoadContext.Default.Unloading += Default_Unloading;
    }

    private static void Default_Unloading(AssemblyLoadContext obj)
    {
        DotNetStarter.Internal.Shutdown.CallShutdown();
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

## Abstractions

All attributes, baseclasses and interfaces reside in the DotNetStarter.Abstractions namespace. Documentation is provided in the intellisense.

## Examples
### Registration
```
public interface ITest
{
    string SayHi(string n);
}

[Register(typeof(ITest))]
public class Test : ITest
{
    public string SayHi(string n) => "Hello " + n;
}
```
### Usage
```
// Import<T> is a struct wrapper for DotNetStarter.ApplicationContext.Default.Locator and can be used when scoping isn't required.
// Also, Import<T> should only be used when Construction Injection is not available.
public Import<ITest> TestService { get; set; }

public void ExampleMethod()
{
    string message = TestService.Service.SayHi("User");
}
```