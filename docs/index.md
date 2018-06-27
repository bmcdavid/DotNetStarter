---
title: DotNetStarter 
---
# DotNetStarter

[![Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter)

The goal of this project is to create a startup and dependency injection framework for netframeworks 3.5+ and netstandard 1.0+
 which allows packages to support container registrations with a container specified at runtime. This allows NuGet package authors to depend on DotNetStarter.Abstractions or DotNetStarter.RegistrationAbstractions taking advantage of dependency injection. Application owners have the ability to install DotNetStarter and a DotNetStarter locator to consume these packages and override package implementations as needed, or let the package provide all the defaults.

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