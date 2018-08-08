---
title: DotNetStarter 
---
[![Master Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de/branch/master?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter/branch/master)

DotNetStarter is a framework for composing applications where many components are provided by NuGet packages. The main audiences of DotNetStarter are package authors and application developers.

Package authors can depend on either the [configuration and startup abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) or the [registration attribute abstractions](https://www.nuget.org/packages/DotNetStarter.RegistrationAbstractions/) to create their components. The components can then be designed with constructor dependency injection in mind. These classes can then be registered by using the [RegistrationAttribute](https://bmcdavid.github.io/DotNetStarter/register.html) or in a startup module implementing [ILocatorConfigure](https://bmcdavid.github.io/DotNetStarter/register.html). Packages may also perform tasks during startup and shutdown using the [IStartupModule](https://bmcdavid.github.io/DotNetStarter/modules.html) interface.

Application developers can install the DotNetStarter package, a [locator](https://bmcdavid.github.io/DotNetStarter/locators.html) (container wrapper) package, any extension such as [MVC](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) for the full ASP.Net framework, and any NuGet packages utilizing the abstractions. Developers have full control over the [startup process](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html) which can be customized through code configuration at almost every level using a fluent configuration API. The framework also supports a wide variety of .NET frameworks from ASP.NET version 3.5 and up, as well as the [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) starting at 1.0.


Package  | Version 
-------- | :------------ 
[DotNetStarter](https://www.nuget.org/packages/DotNetStarter/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.svg)](https://badge.fury.io/nu/DotNetStarter)
[DotNetStarter.Abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Abstractions.svg)](https://badge.fury.io/nu/DotNetStarter.Abstractions)
[DotNetStarter.RegistrationAbstractions](https://www.nuget.org/packages/DotNetStarter.RegistrationAbstractions/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.RegistrationAbstractions.svg)](https://badge.fury.io/nu/DotNetStarter.RegistrationAbstractions)
[DotNetStarter.Web](https://www.nuget.org/packages/DotNetStarter.Web/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Web.svg)](https://badge.fury.io/nu/DotNetStarter.Web)
[DotNetStarter.Owin](https://www.nuget.org/packages/DotNetStarter.Owin/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Owin.svg)](https://badge.fury.io/nu/DotNetStarter.Owin)
[DotNetStarter.DryIoc](https://www.nuget.org/packages/DotNetStarter.DryIoc/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.DryIoc.svg)](https://badge.fury.io/nu/DotNetStarter.DryIoc)
[DotNetStarter.Structuremap](https://www.nuget.org/packages/DotNetStarter.Structuremap/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Structuremap.svg)](https://badge.fury.io/nu/DotNetStarter.Structuremap)
[DotNetStarter.Locators.LightInject](https://www.nuget.org/packages/DotNetStarter.Locators.LightInject/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Locators.LightInject.svg)](https://badge.fury.io/nu/DotNetStarter.Locators.LightInject)
[DotNetStarter.Extensions.Mvc](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Extensions.Mvc.svg)](https://badge.fury.io/nu/DotNetStarter.Extensions.Mvc)
[DotNetStarter.Extensions.WebApi](https://www.nuget.org/packages/DotNetStarter.Extensions.WebApi/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Extensions.WebApi.svg)](https://badge.fury.io/nu/DotNetStarter.Extensions.WebApi)

## Documentation

* [**Important:** Breaking Changes](https://bmcdavid.github.io/DotNetStarter/breaking-changes.html)
* [Customizing Startup](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html)
* [Module creation, discovery, and dependencies](https://bmcdavid.github.io/DotNetStarter/modules.html)
* [Registering items](https://bmcdavid.github.io/DotNetStarter/register.html)
* [Assembly Scanning](https://bmcdavid.github.io/DotNetStarter/scanning.html)
* [Locators](https://bmcdavid.github.io/DotNetStarter/locators.html)
* [Scoped Services](https://bmcdavid.github.io/DotNetStarter/scoped-locator.html)
* [Known Issues](https://bmcdavid.github.io/DotNetStarter/known-issues.html)

## Examples
* [.NET Framework Configuration](https://bmcdavid.github.io/DotNetStarter/example-netfullframework.html)
* [.NET Core Service Configuration](https://bmcdavid.github.io/DotNetStarter/example-netcore-configure-services.html)
* [Episerver Locator with Scoped Dependency Resolver](https://bmcdavid.github.io/DotNetStarter/example-episerver-locator.html)
