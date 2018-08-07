---
title: DotNetStarter - Locators
---
# DotNetStarter - Locators

The ILocator and ILocatorRegistry system is a wrapper for any container that can implement the interfaces.
The following are the currently supported container implementations:

* DotNetStarter.Locators.Autofac - support for net45+ and netstandard1.1+.
* DotNetStarter.Locators.DryIoc - support for net35+ and netstandard 1.0+
* DotNetStarter.Locators.Grace - support for net45+ and netstandard1.0+
* DotNetStarter.Locators.LightInject - support for net452+ and netstandard1.1+.
* DotNetStarter.Locators.Stashbox - support for net40+ and netstandard1.0+.
* DotNetStarter.Locators.StructureMap - support for net45+ and netstandard 1.3+.
* DotNetStarter.Locators.StructureMapSigned - support for net45+.

## Example Custom Locator with factory
A custom locator may also be provided so long as it implements the following interfaces
* ILocator
* ILocatorWithCreateScope - applied to ILocator implementation
* ILocatorScoped
* ILocatorRegistry
* ILocatorRegistryFactory

The custom ILocatorRegistry may then be passed to the override defaults during the StartupBuilder process.
