---
title: DotNetStarter - ILocator Setup
---
# DotNetStarter - Setting up the default ILocator

As of version 1.x of DotNetStarter, an ILocator is required to use DotNetStarter.

The following are the currently supported container implementations:

* [DotNetStarter.DryIoc](http://www.nuget.org/packages/DotNetStarter.DryIoc/) - NuGet package with support for net35+ and netstandard1.3+
* [DotNetStarter.Structuremap](http://www.nuget.org/packages/DotNetStarter.StructureMap/) - NuGet package with support for net35+ and netstandard1.3+, net35 doesn't support container/locator scoping.
* [Custom Locator](https://bmcdavid.github.io/DotNetStarter/custom-locator.html)