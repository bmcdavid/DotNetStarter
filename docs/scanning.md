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

#### The default assembly scanner stores these in a dictionary for retrieval noted below:
```cs
 using DotNetStarter.Abstractions;

[StartupModule]
public class RegistrationConfiguration : ILocatorConfigure
{
    private static readonly Type RegistrationType = typeof(RegistrationAttribute);

    void ILocatorConfigure.Configure(ILocatorRegistry registry, ILocatorConfigureEngine args)
    {
        var scannedRegistrations = args.Configuration.AssemblyScanner.GetTypesFor(RegistrationType);
        var registrations = args.Configuration.DependencySorter
            .Sort<RegistrationAttribute>(scannedRegistrations.OfType<object>())
            .SelectMany(ConvertNodeToRegistration)
            .ToList();

        args.Configuration.RegistrationsModifier?.Modify(registrations);
        registrations.All(r => AddRegistration(r, registry));

        args.Configuration.Environment.Items.Set<ICollection<Registration>>(registrations);
    }

    private bool AddRegistration(Registration registration, ILocatorRegistry registry)
    {
        registry.Add
        (
            registration.ServiceType, // service
            registration.ImplementationType, // implementation
            lifecycle: registration.Lifecycle
        );

        return true;
    }

    private IEnumerable<Registration> ConvertNodeToRegistration(IDependencyNode node)
    {
        var implementationType = node.Node as Type;
        var attrs = implementationType.CustomAttribute(RegistrationType, false).OfType<RegistrationAttribute>();

        return attrs.Select(r => new Registration(r, implementationType));
    }
}
```
