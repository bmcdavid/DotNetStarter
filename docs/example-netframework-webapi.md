---
title: DotNetStarter - WebApi for ASP.Net framework
---
# DotNetStarter - WebApi for ASP.Net framework

Below is an example of how to enable dependency injection in WebApi for ASP.Net.

## NuGet Package
* Install DotNetStarter.Extensions.WebApi
 
### Required Nuget packages [deprecated]

* DotNetStarter
* DotNetStarter.Web
* Microsoft.AspNet.WebApi

```cs
using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;

// adds controller implementations to the assembly scanner
[assembly: DiscoverTypes(typeof(ApiController))]

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

            // then set DependencyResolver in your WebApi config callback
            // GlobalConfiguration.Configure(Register);
            // config.DependencyResolver = new WebApiDependencyResolver(DotNetStarter.ApplicationContext.Default.Locator);
        }

        static void RegisterApiControllers(ILocator locator)
        {
            if (locator == null)
                throw new ArgumentNullException($"{nameof(locator)} cannot be null, please install a locator package such as DotNetStarter.DryIoc or DotNetStart.Structuremap!");

            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = registry.Get<IAssemblyScanner>()?.GetTypesFor(typeof(ApiController));

            foreach (var controller in controllerTypes)
            {
                registry?.Add(controller, controller, lifeTime: LifeTime.Scoped);
            }
        }
    }
}
```