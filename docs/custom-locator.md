---
title: DotNetStarter - Customizing the Locator
---
# DotNetStarter - Customizing the locator

The ILocator and ILocatorRegistry system is a wrapper for any container that can implement the interfaces.
The following are the currently supported container implementations:

* DotNetStarter.Locators.DryIoc - support for net35+ and netstandard 1.0+
* DotNetStarter.Locators.StructureMap - support for net45+ and netstandard 1.3+.
* DotNetStarter.Locators.LightInject - support for net452+ and netstandard1.1+.
* DotNetStarter.Locators.StructureMapSigned - support for net45+.

## Example Custom Locator with factory
```cs
// attribute to register container with the default object factory
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory
(
    typeof(Example.CustomLocatorFactory), // the locator factory to register
    typeof(DotNetStarter.Locators.DryIocLocatorFactory) // factory dependency to override, can have many if needed
)]

namespace Example 
{    
    public class CustomLocatorFactory : ILocatorRegistryFactory
    {
        public ILocatorRegistry CreateRegistry() => new CustomLocator();
    }

    public class CustomLocator : ILocatorRegistry
    {
        // omitted for brevity
    }
}
```