# DotNetStarter.Extensions.WebApi Read Me

To enable, please add the following line to your GlobalConfiguration.Configure(Register) configuration callback. Below is an example of configuring WebApi in an IStartupModule.


```cs
using System.Web.Http;
using DotNetStarter.Abstractions;
using DotNetStarter.Extensions.WebApi;

namespace Example.DotNetStarterModules
{
    [StartupModule]
    public class WebApiConfig : IStartupModule
    {
        public static void Register(HttpConfiguration config, IStartupEngine engine)
        {
            if (engine.Configuration.Environment.IsUnitTest()) { return; }

            // Attribute routing.
            config.MapHttpAttributeRoutes();

            // Assign the dependency resolver
            config.DependencyResolver = new WebApiDependencyResolver(engine.Locator); // never call DotNetStarter.ApplicationContext.Default.Locator in an IStartupModule
        }

        public void Shutdown() { }

        public void Startup(IStartupEngine engine) => GlobalConfiguration.Configure(config => Register(config, engine));
    }
}
```