---
title: DotNetStarter - Breaking Changes
---
# DotNetStarter - Breaking Changes

This page is used to note each versions breaking changes and list future breaking changes.

## Important Note
The read-only locator was introduced in 1.x since registrations after OnLocatorStartupComplete required casting as ILocatorRegistry.

## 1.x - 2.x (proposed)
* Remove ILocator.OpenScope, replace with ILocatorCreateScope which returns an ILocatorScoped instance.
* Add IStartupEnvironment to IStartupConfiguration, and set with similar mechanism to ILocatoryRegistryFactory.
* Remove obsolete methods
 
## 0.x - 1.x breaking changes

 * The following DLLs are now signed: DotNetStarter.Abstractions, DotNetStarter, DotNetStarter.Web, and DotNetStarter.Owin
 * DotNetStarter now requires an [ILocator implementation](https://bmcdavid.github.io/DotNetStarter/ilocator-setup.html) to run.
