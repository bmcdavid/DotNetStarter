---
title: DotNetStarter 
---
# DotNetStarter

[![Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter)

DotNetStarter is a framework for composing applications where many components are provided by NuGet packages. There are two main audiences: package authors and application owners.

Package authors can depend on either the configuration and startup abstractions or the registration attribute abstractions to create their components. The components can then be designed with constructor dependency injection in mind. These classes can then be registered by using the [RegistrationAttribute](https://bmcdavid.github.io/DotNetStarter/register.html) or in a startup module implementing [ILocatorConfigure](https://bmcdavid.github.io/DotNetStarter/register.html). Packages may also perform tasks during startup and shutdown using the [IStartupModule](https://bmcdavid.github.io/DotNetStarter/modules.html) interface.

Application owners can install the DotNetStarter package, a locator (container wrapper) package, any extension such as MVC for the full ASP.Net framework, and any NuGet packages utilizing the abstractions. Owners have full control over the [startup process](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html) which can be customized through code configuration at almost every level using a fluent configuration API. The framework also supports a wide variety of .NET frameworks, specifically ASP.NET version 3.5+ and the [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) starting at 1.0.

Package  | Version 
-------- | :------------ 
[DotNetStarter](https://www.nuget.org/packages/DotNetStarter/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.svg)](https://badge.fury.io/nu/DotNetStarter)
[DotNetStarter.Abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Abstractions.svg)](https://badge.fury.io/nu/DotNetStarter.Abstractions)
[DotNetStarter.RegistrationAbstractions](https://www.nuget.org/packages/DotNetStarter.RegistrationAbstractions/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.RegistrationAbstractions.svg)](https://badge.fury.io/nu/DotNetStarter.RegistrationAbstractions)
[DotNetStarter.Web](https://www.nuget.org/packages/DotNetStarter.Web/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Web.svg)](https://badge.fury.io/nu/DotNetStarter.Web)
[DotNetStarter.Owin](https://www.nuget.org/packages/DotNetStarter.Owin/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Owin.svg)](https://badge.fury.io/nu/DotNetStarter.Owin)
[DotNetStarter.DryIoc](https://www.nuget.org/packages/DotNetStarter.DryIoc/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.DryIoc.svg)](https://badge.fury.io/nu/DotNetStarter.DryIoc)
[DotNetStarter.Structuremap](https://www.nuget.org/packages/DotNetStarter.Structuremap/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Structuremap.svg)](https://badge.fury.io/nu/DotNetStarter.Structuremap)
[DotNetStarter.Extensions.Mvc](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Extensions.Mvc.svg)](https://badge.fury.io/nu/DotNetStarter.Extensions.Mvc)
[DotNetStarter.Extensions.WebApi](https://www.nuget.org/packages/DotNetStarter.Extensions.WebApi/) |  [![NuGet version](https://badge.fury.io/nu/DotNetStarter.Extensions.WebApi.svg)](https://badge.fury.io/nu/DotNetStarter.Extensions.WebApi)

## Documentation

* [**Important:** Breaking Changes](https://bmcdavid.github.io/DotNetStarter/breaking-changes.html)
* [Customization](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html)
* [Module creation, discovery, and dependencies](https://bmcdavid.github.io/DotNetStarter/modules.html)
* [Registering items](https://bmcdavid.github.io/DotNetStarter/register.html)
* [Assembly Scanning](https://bmcdavid.github.io/DotNetStarter/scanning.html)
* [Custom Locator](https://bmcdavid.github.io/DotNetStarter/custom-locator.html)
* [Scoped Services](https://bmcdavid.github.io/DotNetStarter/scoped-locator.html)
* [Known Issues](https://bmcdavid.github.io/DotNetStarter/known-issues.html)

## Examples
* [Episerver Locator with Scoped Dependency Resolver](https://bmcdavid.github.io/DotNetStarter/example-episerver-locator.html)
* [ASP.Net Core Service Configuration](https://bmcdavid.github.io/DotNetStarter/example-netcore-configure-services.html)