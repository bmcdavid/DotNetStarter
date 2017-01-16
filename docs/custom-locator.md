---
title: DotNetStarter - Customizing the Locator
---
# DotNetStarter - Customizing the locator

The ILocator and ILocatorRegistry system is a wrapper for any container that can implement the interfaces.
The following are the currently supported container implementations:

* DotNetStarter.DryIoc - support for net35+ and netstandard1.3+
* DotNetStarter.Structuremap - support for net35+ and netstandard1.3+, net35 doesn't support container/locator scoping.

## Example Custom Locator with factory

```
// attribute to register container with the default object factory
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory
(
    typeof(Example.CustomLocatorFactory), // the locator factory to register
    typeof(DotNetStarter.DryIocLocatorFactory) // factory dependency to override, can have many if needed
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

