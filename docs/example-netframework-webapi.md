---
title: DotNetStarter - WebApi for ASP.Net framework
---
# DotNetStarter - WebApi for ASP.Net framework

Below is an example of how to enable dependency injection in WebApi for ASP.Net.

### Required Nuget packages

* DotNetStarter
* DotNetStarter.Web
* Microsoft.AspNet.WebApi

```
using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;

// adds controller implementations to the assembly scanner
[assembly: ScanTypeRegistry(typeof(ApiController))]

namespace WebFrameworkApp.App_Start
{
    public class WebApiDependencyResolver : IDependencyResolver
    {
        ILocator _Locator;

        public WebApiDependencyResolver(ILocator locator)
        {
            _Locator = locator;
        }

        public IDependencyScope BeginScope()
        {
            return new WebApiDependencyResolver(_Locator.OpenScope());
        }

        public void Dispose()
        {
            _Locator?.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return (HttpContext.Current?.GetScopedLocator() ?? _Locator).Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (HttpContext.Current?.GetScopedLocator() ?? _Locator).GetAll(serviceType);
        }
    }

    [StartupModule]
    public class WebApiLocatorStartup : IStartupModule
    {
        public void Shutdown(IStartupEngine engine) { }

        public void Startup(IStartupEngine engine)
        {
            RegisterApiControllers(engine.Locator);

            GlobalConfiguration.Configure((config) => Register(config, engine.Locator));
        }

        public static void RegisterApiControllers(ILocator locator)
        {
            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = registry.Get<IAssemblyScanner>()?.GetTypesFor(typeof(ApiController));
            
            foreach (var controller in controllerTypes)
            {
                registry?.Add(controller, controller, lifeTime: LifeTime.Scoped);
            }
        }

        public static void Register(HttpConfiguration config, ILocator locator)
        {
            config.DependencyResolver = new WebApiDependencyResolver(locator);

            // modify rest as needed
            config.MapHttpAttributeRoutes();
        }
    }
}
```