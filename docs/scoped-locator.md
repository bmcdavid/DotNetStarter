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
```cs
var locator = DotNetStarter.ApplicationContext.Default.Locator;

using (var scope = locator.OpenScope())
{
    var scopedService = scope.Get<IScopedService>();

    // do scoped work
}
```

### Creating a scope for System.Web requests
Install package DotNetStarter.Web. The scoped locator can be retrieved with

```cs
var scopedLocator = HttpContext.Current?.GetScopedLocator(); // using DotNetStarter.Web;
```

### Adding aspnetcore services to Locator for scoped IServiceProvider
See [examples for netcore](https://bmcdavid.github.io/DotNetStarter/example-netcore-configure-services.html)