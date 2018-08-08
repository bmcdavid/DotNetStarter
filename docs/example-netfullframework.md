---
title: DotNetStarter - .NET Framework Web Configuration
---
# DotNetStarter - .NET Framework Web Configuration

Below is an example of how to configure DotNetStarter with the .NET framework. Optionally, MVC and WebApi extension packages also exist to enable controller resolution during startup if installed with notes in below sample code.

## Required NuGet packages

* DotNetStarter
* Any [supported locator](https://bmcdavid.github.io/DotNetStarter/locators.html) package

```cs
using DotNetStarter.Abstractions;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ExampleApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            // Executed in Global.asax constructor to allow IHttpModule startup modules to register
            DotNetStarter.Configure.StartupBuilder.Create()
                //environment allows for conditional check to swap services, perform tasks only in production, etc
                .UseEnvironment(new DotNetStarter.StartupEnvironment(ConfigurationManager.AppSettings["DotNetStarter.Environment"]))
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                    .WithDiscoverableAssemblies()
                    .WithAssemblyFromType<MvcApplication>();
                })
                //.Build() // executes all ILocatorConfigure instances
                .Run(); // executes all IStartupModule instances


            // tip: Install DotNetStarter.Extensions.Mvc to enable MVC controller injection, no extra configuration needed
            // tip: Install DotNetStarter.Extensions.WebApi to use WebApi controller injected, see WebApiConfig class below to enable
        }


        // tip: These can be converted to IStartupModule implementations and removed from Application_Start
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        [StartupModule]
        public class WebApiConfig : IStartupModule
        {
            public static void Register(HttpConfiguration config, IStartupEngine engine)
            {
                if (engine.Configuration.Environment.IsUnitTest()) { return; }

                // Attribute routing.
                config.MapHttpAttributeRoutes();

                // Assign the ILocator as the dependency resolver
                config.DependencyResolver = new DotNetStarter.Extensions.WebApi.WebApiDependencyResolver(engine.Locator);
            }

            void IStartupModule.Shutdown() { }

            /// <summary>
            /// Configure WebApi GlobalConfiguration
            /// </summary>
            /// <param name="engine"></param>
            public void Startup(IStartupEngine engine) => GlobalConfiguration.Configure(config => Register(config, engine));
        }
    }
}
```