---
title: DotNetStarter - Customizing the object factory
---
# DotNetStarter - Customizing the object factory

The object factory is responsible for creating objects before the Ioc/DI container is setup. 
Implementations must use the IStartupObjectFactory interface, which is typically done by inheriting the default implentation
 and overriding the desired method to customize. 

***Important:*** Object factories use a sort order property, the highest value wins. The default factory has a value of 0.

## Example Custom Object Factory

```
// attribute to register 
[assembly: StartupObjectFactory(typeof(Example.CustomObjectFactory))]

namespace Example 
{    
    public class CustomLocatorFactory : DotNetStarter.StartupObjectFactory
    {
        // set higher than base
        public override int SortOrder => base.SortOrder + 1;
    
        // omitted for brevity, but override as needed
    }
}
```