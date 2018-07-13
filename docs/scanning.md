---
title: DotNetStarter - Assembly Scanning
---
# DotNetStarter - Assembly Scanning

Assembly scanning occurs to discover service registrations (RegistrationAttribute) and IStartupModule/ILocatorConfigure modules,
 but can also be extended with the DiscoverTypesAttribute for assemblies. 

#### Below is the default for DotNetStarter to discover its needed types:
```cs
 using DotNetStarter.Abstractions;

 // this scans for types that implement IStartupModule, StartupModuleAttribute, and RegistrationAttribute usages
[assembly: DiscoverTypes(
    typeof(IStartupModule),
    typeof(StartupModuleAttribute),
    typeof(RegistrationAttribute)
)]
```

#### The default assembly scanner stores these in a static dictionary for retrieval noted below:
```cs
 using DotNetStarter.Abstractions;

[StartupModule]
public class RegistrationConfiguration : ILocatorConfigure
{
    public void Configure(ILocatorRegistry container, IStartupEngine engine)
    {
        // access to default configuration's to get its AssemblyScanner
        var configuration = engine.Configuration;
        var serviceType = typeof(RegistrationAttribute);
        
        // get all types found for RegistrationAttribute
        var services = configuration.AssemblyScanner.GetTypesFor(serviceType);

        // additional sorting used since RegistrationAttribute inherits StartupDependencyBaseAttribute which allows dependency system
        var servicesSorted = configuration.DependencySorter.Sort<RegistrationAttribute>(services.OfType<object>()).ToList();

        // add all types found
        for (int i = 0; i < servicesSorted.Count; i++)
        {
            var t = servicesSorted[i].Node as Type;
            var attrs = t.CustomAttribute(serviceType, false).OfType<RegistrationAttribute>();

            if (attrs?.Any() == true)
            {
                foreach (var attr in attrs)
                {
                    attr.ImplementationType = t;
                    container.Add(attr.ServiceType, attr.ImplementationType, null, attr.Lifecycle);
                }
            }
        }
    }
}
```
