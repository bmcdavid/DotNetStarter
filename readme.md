# DotNetStarter Read Me

[![Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter)

This package supports native .netframeworks 3.5, 4.0, and 4.5 as well as netstandard 1.3.

The goal of this package to create a startup framework for any dotnet project where everything is swappable via Inversion Of Control (IoC) or object factories (AssemblyFactoryBaseAttribute).

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

* netcoreapps require a custom IAssemblyLoader noted below:
```
    /// <summary>
    /// Assigned by AssemblyLoader.SetAssemblyLoader(new WebAssemblyLoader()); as first line in Program.cs
    /// </summary>
    public class WebAssemblyLoader : IAssemblyLoader
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            var runtimeId = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
            var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames(Microsoft.Extensions.DependencyModel.DependencyContext.Default, runtimeId);

            return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
        }
    }

```
## Abstractions

All attributes, baseclasses and interfaces reside in the DotNetStarter.Abstractions namespace. Documentation is provided in the intellisense.

## Examples of DI/IOC, requires an ILocator package such as DotNetStarter.DryIoc or DotNetStarter.StructureMap
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
	private Import<ITest> TestService;
	// Import<T> is preferred over DotNetStarter.Context.Default.Locator.Get<T>(), unless you are in a factory context of needing new instances.
        
    public void ExampleMethod()
    {
        string message = TestService.Service.SayHi("User");
    }
```