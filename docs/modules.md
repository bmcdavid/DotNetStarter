---
title: DotNetStarter - Modules
---
# DotNetStarter - Modules

DotNetStarter contains two main module types:
* IStartupModule - a startup/shutdown system with support for DI but no scoped registrations.
* [ILocatorConfigure](./register.html) -a system for configuration locator services requires empty constructors.

## IStartupModules
Startup modules will execute a Startup method when either DotNetStarter.Context.Startup() is executed or DotNetStarter.Context.Default.Locator is accessed. 
This call should be placed early in the application, below are a few possible places to execute:

* ASP.Net WebApp - use Application_Start in the global.asax
* AspNetCore - use in the startup class ConfigureServices or constructor.
* Console apps - use as first line in Main method.
* Owin apps - use app.UseScopedLocator(DotNetStarter.Context.Default.Locator) as first middleware.

***Note:*** They Shutdown method may not execute by default in all systems as noted in the [known issues](./known-issues.html).

## Filtering Modules