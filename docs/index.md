---
title: DotNetStarter 
---
# DotNetStarter

[![Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter)

The goal of this project is to create a startup and IoC/DI framework for netframeworks 3.5+ and netstandard 1.3+
 which allows packages to support container registrations with a container specified at runtime.

 Package  | Version |
-------- | :------------ | :------------------
[DotNetStarter](https://www.nuget.org/packages/DotNetStarter/) |  [![DotNetStarter](https://img.shields.io/nuget/v/DotNetStarter.svg)](https://www.nuget.org/packages/DotNetStarter/) |
[DotNetStarter.Abstractions](https://www.nuget.org/packages/DotNetStarter.Abstractions/) |  [![DotNetStarter.Abstractions](https://img.shields.io/nuget/v/DotNetStarter.Abstractions.svg)](https://www.nuget.org/packages/DotNetStarter.Abstractions/) |
[DotNetStarter.Web](https://www.nuget.org/packages/DotNetStarter.Web/) |  [![DotNetStarter.Web](https://img.shields.io/nuget/v/DotNetStarter.Web.svg)](https://www.nuget.org/packages/DotNetStarter.Web/) |
[DotNetStarter.DryIoc](https://www.nuget.org/packages/DotNetStarter.DryIoc/) |  [![NuGet](https://img.shields.io/nuget/v/DotNetStarter.DryIoc.svg)](https://www.nuget.org/packages/DotNetStarter.DryIoc/) |
[DotNetStarter.Structuremap](https://www.nuget.org/packages/DotNetStarter.Structuremap/) |  [![DotNetStarter.Structuremap](https://img.shields.io/nuget/v/DotNetStarter.Structuremap.svg)](https://www.nuget.org/packages/DotNetStarter.Structuremap/) |
[DotNetStarter.Extensions.Mvc](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) |  [![DotNetStarter.Extensions.Mvc](https://img.shields.io/nuget/v/DotNetStarter.Extensions.Mvc.svg)](https://www.nuget.org/packages/DotNetStarter.Extensions.Mvc/) |
[DotNetStarter.Extensions.WebApi](https://www.nuget.org/packages/DotNetStarter.Extensions.WebApi/) |  [![DotNetStarter.Extensions.WebApi](https://img.shields.io/nuget/v/DotNetStarter.Extensions.WebApi.svg)](https://www.nuget.org/packages/DotNetStarter.Extensions.WebApi/) |

## Documentation

* [Object Factory and customization](https://bmcdavid.github.io/DotNetStarter/custom-objectfactory.html)
* [Module creation, discovery, and dependencies](https://bmcdavid.github.io/DotNetStarter/modules.html)
* [Registering items](https://bmcdavid.github.io/DotNetStarter/register.html)
* [Assembly Scanning](https://bmcdavid.github.io/DotNetStarter/scanning.html)
* [Custom Locator](https://bmcdavid.github.io/DotNetStarter/custom-locator.html)
* [Scoped Services](https://bmcdavid.github.io/DotNetStarter/scoped-locator.html)
* [Known Issues](https://bmcdavid.github.io/DotNetStarter/known-issues.html)

## Examples
* [MVC with Scoped Dependency Resolver](https://bmcdavid.github.io/DotNetStarter/example-netframework-mvc.html)
* [WebApi with Scoped Depenendy Resolver](https://bmcdavid.github.io/DotNetStarter/example-netframework-webapi.html)
* [Episerver Locator with Scoped Dependency Resolver](https://bmcdavid.github.io/DotNetStarter/example-episerver-locator.html)
* [ASP.Net Core Service Configuration](https://bmcdavid.github.io/DotNetStarter/example-netcore-configure-services.html)