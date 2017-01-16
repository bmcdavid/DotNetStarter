---
title: DotNetStarter - Assembly Scanning
---
# DotNetStarter - Assembly Scanning

Assembly scanning occurs to discover IStartupModule and ILocatorConfigure modules,
 but can also be extended with the ScanTypeRegistryAttribute for assemblies. 

#### Below is the default for DotNetStarter to discover its needed types:

 ```

 // this scans for types that implement IStartupModule, StartupModuleAttribute, and RegisterAttribute usages
[assembly: ScanTypeRegistry(
    typeof(IStartupModule),
    typeof(StartupModuleAttribute),
    typeof(RegisterAttribute)
)]
 ```

#### The default assembly scanner stores these in a static dictionary for retrieval noted below:
 
 ```

[StartupModule]
public class RegisterConfiguration : ILocatorConfigure
{
    public void Configure(ILocatorRegistry container, IStartupEngine engine)
    {
        // access to default configuration's to get its AssemblyScanner
        var configuration = container.Get<IStartupConfiguration>() ?? engine.Configuration;
        var serviceType = typeof(RegisterAttribute);
        
        // get all types found for RegisterAttribute
        var services = configuration.AssemblyScanner.GetTypesFor(serviceType);

        // additional sorting used since RegisterAttribute inherits DependencyBaseAttribute which allows dependency system
        var servicesSorted = configuration.DependencySorter.Sort<RegisterAttribute>(services.OfType<object>()).ToList();

        // add all types found
        for (int i = 0; i < servicesSorted.Count; i++)
        {
            var t = servicesSorted[i].Node as Type;
            var attrs = t.CustomAttribute(serviceType, false).OfType<RegisterAttribute>();

            if (attrs?.Any() == true)
            {
                foreach (var attr in attrs)
                {
                    attr.ImplementationType = t;
                    container.Add(attr.ServiceType, attr.ImplementationType, null, attr.LifeTime, attr.ConstructorType);
                }
            }
        }
    }
}

 ```

