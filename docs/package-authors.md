---
title: DotNetStarter - Package Authors
---
# DotNetStarter - Package Authors

Package authors can depend on either the [configuration and startup abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) or the [registration attribute abstractions](https://www.nuget.org/packages/DotNetStarter.RegistrationAbstractions/) to create their components. The components can then be designed with constructor dependency injection in mind. These classes can then be registered by using the [RegistrationAttribute](https://bmcdavid.github.io/DotNetStarter/register.html) or in a startup module implementing [ILocatorConfigure](https://bmcdavid.github.io/DotNetStarter/register.html). Packages may also perform tasks during startup and shutdown using the [IStartupModule](https://bmcdavid.github.io/DotNetStarter/modules.html) interface.

## Best Practices

For any packages, adding the below assembly attribute in an **AssemblyInfo.cs** is a best practice, as application owners can easily find modules within the package during the scanning process. The attribute also simplifies the startup builder configuration.

```cs
[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
```

### More best practices

* Do not access the container in the ILocatorRegistry interface.
* Do not resolve/get services during ILocatorConfigure.Configure.
* Do provide interfaces for package services.
* Do use constructor injection for package services.