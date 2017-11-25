---
title: DotNetStarter - Known Issues
---
# Known Issues

#### IStartupModule.Shutdown doesn't execute in netcoreapps. Workaround is to add an init module and attach to unloading event as noted below:
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

#### netcoreapps require custom assembly loading noted below:
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