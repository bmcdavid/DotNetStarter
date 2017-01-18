---
title: DotNetStarter - MVC for ASP.Net framework
---
# DotNetStarter - MVC for ASP.Net framework

Below is an example of how to enable dependency injection in MVC for ASP.Net.

### Required Nuget packages

* DotNetStarter
* DotNetStarter.Web
* Microsoft.AspNet.Mvc

```
using DotNetStarter.Abstractions;
using DotNetStarter.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// adds controller implementations to the assembly scanner
[assembly: ScanTypeRegistry(typeof(IController))]

namespace Example
{
    /// <summary>
    /// Requires DotNetStarter.Web and Microsoft.AspNet.Mvc packages
    /// </summary>
    public class ScopedDependencyResolverViaHttpContext : IDependencyResolver
    {
        ILocator _Locator;

        public ScopedDependencyResolverViaHttpContext(ILocator locator)
        {
            _Locator = locator;
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

    /// <summary>
    /// executes on DotNetStarter startup to set MVC dependency resolver and register controllers
    /// </summary>
    [StartupModule]
    public class MvcDependencyResolverSetup : IStartupModule
    {
        ILocator _Locator;

        public MvcDependencyResolverSetup(ILocator locator)
        {
            _Locator = locator;
        }

        public void Shutdown(IStartupEngine engine) { }

        public void Startup(IStartupEngine engine)
        {
            RegisterMvcControllers(engine.Locator);
            
            DependencyResolver.SetResolver(new ScopedDependencyResolverViaHttpContext(engine.Locator));
        }

        public static void RegisterMvcControllers(ILocator locator)
        {
            var registry = locator as ILocatorRegistry;
            IEnumerable<Type> controllerTypes = locator.Get<IAssemblyScanner>()?.GetTypesFor(typeof(IController));
            
            foreach (var controller in controllerTypes.Where(x => !x.IsAbstract && !x.IsInterface))
            {
                registry?.Add(controller, controller, lifeTime: LifeTime.Scoped);
            }
        }
    }
}
```