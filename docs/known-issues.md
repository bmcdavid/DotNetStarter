---
title: DotNetStarter - Known Issues
---
# Known Issues

#### IStartupModule.Shutdown doesn't execute in netcoreapps. Workaround is to add an init module and attach to unloading event as noted below:
```cs
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