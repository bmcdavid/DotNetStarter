---
title: DotNetStarter - Registering Items
---
# DotNetStarter - Registering Items

There are two ways to register items with to the locator.

## DotNetStarter.Abstractions.RegistrationAttribute

The first and simplest way is using the DotNetStarter.Abstractions.RegistrationAttribute on the implementation class.
This works for registering type implementations but doesn't support delegate or instance based registrations.

```cs
[Registration(typeof(IServiceType), LifeTime.Singleton)]
```

### Customizing RegistrationAttribute lifecycles
Application developers may modify services discovered with the RegistrationAttribute in the StartupBuilder.OverrideDefaults callback using the UseRegistrationModifier to pass a custom registration modifier. The default is null. Below is a sample registration modifier

```cs
public class AppRegistrationModifier : IRegistrationsModifier
{
    public void Modify(ICollection<Registration> registrations)
    {
        // changes all IFooService to singletons
        registrations.Where(r => r.ServiceType == typeof(IFooService)).All(r =>
        {
            r.Lifecycle = Lifecycle.Singleton;
            return true;
        });
    }
}
```

## DotNetStarter.Abstractions.ILocatorConfigure
The second method requires a class that implements DotNetStarter.Abstractions.ILocatorConfigure with a StartupModuleAttribute decoration.

```cs
[StartupModule]
public class Example : ILocatorConfigure
{
    public void Configure(ILocatorRegistry container, ILocatorConfigureEngine engine)
    {
        container.Add<BaseTest, BaseImpl>(lifecycle: Lifecycle.Scoped);

        container.Add(typeof(IFoo), locator => FooFactory.CreateFoo(), Lifecycle.Transient);
    }
}
```

***IMPORTANT:*** The types that implement this interface also need empty constructors as locator with its underlying container have not been configured, there will be nothing to inject.

## Dependencies
Both methods support adding dependencies which allows for an override system. The most common example will be overriding services registered with the RegistrationAttribute

### ILocatorConfigure module dependent on the RegisterConfiguration type:

```cs
[StartupModule(typeof(RegisterConfiguration))]
public class DependencyExample : ILocatorConfigure
{
    public void Configure(ILocatorRegistry container, ILocatorConfigureEngine engine)
    {
        container.Add<IServiceType, NewImpl>(lifecycle: Lifecycle.Scoped);
    }
}
```

### RegistrationAttribute dependent on another implementation type:

```cs
[Registration(typeof(IServiceType), Lifecycle.Singleton, typeof(ServiceTypeImplToOverride))]
```

The attribute with the typeof(RegistrationConfiguration) allows this ILocatorConfigure to be executed after the RegistrationAttribute assignments. 
The StartupModuleAttribute and RegistrationAttribute a for an array of type dependencies, so the more dependencies they have the later they will executed! 

***IMPORTANT:*** The typeof() dependencies must match the class type they are overriding, for example RegistrationConfiguration implements ILocatorConfigure,
 so adding it as dependency to implementations of ILocatorConfigure works.
Adding a type that doesn't match for example typeof(string) will result in an InvalidOperationException during dependency sorting.
