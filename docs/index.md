---
title: DotNetStarter 
---
# DotNetStarter

[![Build status](https://ci.appveyor.com/api/projects/status/a907wfniy73sk5de?svg=true)](https://ci.appveyor.com/project/bmcdavid/dotnetstarter)

DotNetStarter is a startup framework for creating modules spread across assemblies hooked to startup and shutdown events. This package also provides and ILocatorRegistry which adds dependency injection (DI)/ inversion of control (ioc). A locator package is required to enable DI/IOC functionality such as DotNetStarter.DryIoc or DotNetStarter.StructureMap, and is highly encouraged. 

This package supports native .netframeworks 3.5, 4.0, and 4.5 as well as netstandard 1.3.

* [Module creation, discovery, and dependencies](./modules.html)
* [Registering items](./register.html)
* [Assembly Scanning](./scanning.html)
* [Custom Locator](./custom-locator.html)
* [Known Issues](./known-issues.html)
