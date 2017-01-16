---
title: DotNetStarter - Scoped Services
---
# DotNetStarter - Scoped Services

Container scoping allows for a single instance to be used until the scope is disposed. The most common use is an HTTP web request. 

In aspnetcore apps, the provided HttpContext.RequestServices is a scoped IServiceProvider. 

***Important:*** Although app.UseMiddleware supports DI for service injection, its scoped instances will not be the same as the HttpContext.RequestServices. In order to use HttpContext scoped items, they must be retrieved in the middleware's Invoke method!
Also some containers may handle scoped items differently, some may have a singleton instance when no scoping is available, others may throw exceptions or return null.

## Examples

### Creating a scope
```
var locator = DotNetStarter.Context.Default.Locator;

using (var scope = locator.OpenScope())
{
    var scopedService = scope.Get<IScopedService>();

    // do scoped work
}
```

### Creating a scope for System.Web requests
To do

### Adding aspnetcore services to Locator for scoped IServiceProvider
```
// using DotNetStarter.Owin;

public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddScoped(typeof(IScopedService), typeof(ScopedServiceImpl));

    return services.AddServicesToLocator(DotNetStarter.Context.Default.Locator as ILocatorRegistry);
}
```