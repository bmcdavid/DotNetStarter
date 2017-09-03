---
title: DotNetStarter - Breaking Changes
---
# DotNetStarter - Breaking Changes

This page is used to note each versions breaking changes and list future breaking changes.

## 1.x - 2.x (proposed)
* Change NuGet dependencies to use NETStandard.Library instead of individual package references.
* Change ILocator.OpenScope signature to return ILocatorRegistry and take no parameters
* Add IReadOnlyLocator which locks container once SetContainer is called, or EnsureLocked() is executed.
* Add IStartupEnvironment to IStartupConfiguration, and set with similar mechanism to ILocatoryRegistryFactory.
* Remove always unique lifetime.
 
## 0.x - 1.x breaking changes

 * The following DLLs are now signed: DotNetStarter.Abstractions, DotNetStarter, DotNetStarter.Web, and DotNetStarter.Owin
 * DotNetStarter now requires an [ILocator implementation](https://bmcdavid.github.io/DotNetStarter/ilocator-setup.html) to run.
