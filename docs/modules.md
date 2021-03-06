---
title: DotNetStarter - Modules
---
# DotNetStarter - Modules

DotNetStarter module types:

* IStartupModule: startup/shutdown system with support for DI but no scoped registrations.
* [ILocatorConfigure](https://bmcdavid.github.io/DotNetStarter/register.html): system for configurating locator/container services which requires empty constructors for implementations;
* IWebModule: startup module that implements IHttpModule in DotNetStarter.Web (only for netframework projects).

## IStartupModules
Startup modules will execute a Startup method when either the StartupBuilder.Run() is executed or DotNetStarter.ApplicationContext.Default.Locator is accessed. 
This call should be placed early in the application, below are a few possible places to execute:

* ASP.Net WebApp - use in constructor in the global.asax code class.
* AspNetCore - use in the startup class ConfigureServices or constructor.
* Console apps - use as first line in Main method.
* Owin apps - use app.UseScopedLocator(DotNetStarter.ApplicationContext.Default.Locator) as first middleware.

***Note:*** The Shutdown method may not execute by default in all systems as noted in the [known issues](https://bmcdavid.github.io/DotNetStarter/known-issues.html).

## Filtering Modules
Modules can be removed during startup using the StartupModuleExpression in the StartupBuilder.ConfigureStartupModules callback. An example can be found in the [customization fine-tuned example](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html).

## Startup Module Example
```cs
using DotNetStarter.Abstractions;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace Example
{
    public interface IMessageLogger
    {
        void LogMessage(string message);
    }

    [Register(typeof(IMessageLogger), LifeTime.Singleton)]
    public class MessageLogger : IMessageLogger
    {
        private StringBuilder LogBuilder = new StringBuilder();

        public void LogMessage(string message)
        {
            LogBuilder.AppendLine(message);
        }

        public override string ToString()
        {
            return LogBuilder.ToString();
        }
    }

    [StartupModule] // supports type() dependencies
    public class ExampleStartupModule : IStartupModule
    {
        IMessageLogger _Logger;

        /// <summary>
        /// Contstructors support dependency injection!
        /// </summary>
        /// <param name="logger"></param>
        public ExampleStartupModule(IMessageLogger logger)
        {
            _Logger = logger;
        }

        public void Shutdown()
        {
            _Logger.LogMessage("Shutting down!");

            File.WriteAllText(HostingEnvironment.MapPath("~/MessageLogger.txt"), _Logger.ToString());
        }

        public void Startup(IStartupEngine engine)
        {
            _Logger.LogMessage("Starting up!");
        }
    }
}
```