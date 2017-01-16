---
title: DotNetStarter - Registering Items
---
# DotNetStarter - Registering Items

There are two ways to register items with to the locator.

## DotNetStarter.Abstractions.RegisterAttribute

The first and simplest way is using the DotNetStarter.Abstractions.RegisterAttribute on the implementation class.
This works for registering type implementations but doesn't support delegate or instance based registrations.

```
[Register(typeof(IServiceType), LifeTime.Singleton)]
```

## DotNetStarter.Abstractions.ILocatorConfigure

The second method requires a class that implements DotNetStarter.Abstractions.ILocatorConfigure with a StartupModuleAttribute decoration.

```
[StartupModule]
public class Example : ILocatorConfigure
{
    public void Configure(ILocatorRegistry container, IStartupEngine engine)
    {
        container.Add<BaseTest, BaseImpl>(lifetime: LifeTime.Scoped);

        container.Add(typeof(IFoo), locator => FooFactory.CreateFoo(), LifeTime.Transient);
    }
}
```


***IMPORTANT:*** The types that implement this interface also need empty constructors as locator with its underlying container have not been configured, there will be nothing to inject.

## Dependencies
Both methods support adding dependencies which allows for an override system. The most common example will be overriding services registered with the RegisterAttribute

Below is an ILocatorConfigure module dependent on the RegisterConfiguration type:

```
[StartupModule(typeof(RegisterConfiguration))]
public class DependencyExample : ILocatorConfigure
{
    public void Configure(ILocatorRegistry container, IStartupEngine engine)
    {
        container.Add<IServiceType, NewImpl>(lifetime: LifeTime.Scoped);
    }
}
```

Below is a RegisterAttribute dependent on another implementation type:

```
[Register(typeof(IServiceType), LifeTime.Singleton, Contructor.Greediest, typeof(ServiceTypeImplToOverride))]
```

The attribute with the typeof(RegisterConfiguration) allows this ILocatorConfigure to be executed after the RegisterAttribute assignments. 
The StartupModuleAttribute and RegisterAttribute a for an array of type dependencies, so the more dependencies they have the later they will executed! 


***IMPORTANT:*** The typeof() dependencies must match the class type they are overriding, for example RegisterConfiguration implements ILocatorConfigure,
 so adding it as dependency to implementations of ILocatorConfigure works.
Adding a type that doesn't match for example typeof(string) will result in an InvalidOperationException during dependency sorting.
