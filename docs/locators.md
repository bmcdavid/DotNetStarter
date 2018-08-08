---
title: DotNetStarter - Locators
---
# DotNetStarter - Locators

A locator package must be installed to enable the ILocator and use DotNetStarter. The ILocator and ILocatorRegistry system is a wrapper for any container that can implement the interfaces.
The following are the currently supported container implementations:

* DotNetStarter.Locators.Autofac - support for net45+ and netstandard1.1+.
* DotNetStarter.Locators.DryIoc - support for net35+ and netstandard 1.0+
* DotNetStarter.Locators.Grace - support for net45+ and netstandard1.0+
* DotNetStarter.Locators.LightInject - support for net452+ and netstandard1.1+.
* DotNetStarter.Locators.Stashbox - support for net40+ and netstandard1.0+.
* DotNetStarter.Locators.StructureMap - support for net45+ and netstandard 1.3+.
* DotNetStarter.Locators.StructureMapSigned - support for net45+.

To select a locator the application developer by overriding the default as noted below:

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

## Custom Locator
A custom locator may also be provided so long as it implements the following interfaces
* ILocator
* ILocatorWithCreateScope - applied to ILocator implementation
* ILocatorScoped
* ILocatorRegistry
* ILocatorRegistryFactory

The custom ILocatorRegistry may then be passed to the override defaults during the StartupBuilder process.

### Optional features
Features can be added to ILocator and ILocatorRegistry implementations with addtional interfaces as noted below:

* ILocatorRegistryWithContains
* ILocatorRegistryWithRemove
* ILocatorWithPropertyInjection
* ILocatorWithDebugInfo

Package authors should avoid using the optional features in packages, as they are intended for application developers.