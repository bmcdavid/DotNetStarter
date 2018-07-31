---
title: DotNetStarter - Breaking Changes
---
# DotNetStarter - Breaking Changes

This page is used to note each versions breaking changes and list future breaking changes.

## Important Note
The read-only locator was introduced in 1.x since registrations after OnLocatorStartupComplete required casting as ILocatorRegistry.

## 2.x - 3.x (Proposed)
* Remove IStartupObjectFactory
* Merge IlocatorDefaultRegistrationsWithCollections into IlocatorDefaultRegistrations.
* Remove DotNetStarter.ApplicationContext.Startup methods
* Remove IStartupConfigurationWithEnvironment
* Changed IStartupHandler interface.

## 1.x - 2.x
* Added IStartupEnvironment to IStartupConfiguration, and set by passing an implementation to DotNetStarter.ApplicationContext.Startup
* Added LightInject and StructureMapSigned locators
* Added DotNetStarter.Locators namespace for all locators
  * DryIoc and Structuremap retain their existing NuGet package Ids
* New services can no longer be registered in an open scope
* Removed obsoleted code
* Removed ILocator inheritance from ILocatorRegistry
* Renamed IShutdownHandler.InvokeShutdown to Shutdown
* Removed IStartupModule.Shutdown IStartupEngine arguments
* Removed RegisterAttribute, replaced with RegistrationAttribute
* Removed LifeTime and ConstructorType enums
* Moved RegistrationConfiguration to DotNetStarter.Abstractions
* Changed ILocator
  * Removed OpenScope
  * Moved BuildUp to ILocatorWithPropertyInjection
* Changed ILocatorRegistry interface
  * Changed method signature to account for removed enums
  * Moved Remove to ILocatorRegistryWithRemove
  * Moved Contains to ILocatorRegistryWithContains
 
## 0.x - 1.x breaking changes

 * The following DLLs are now signed: DotNetStarter.Abstractions, DotNetStarter, DotNetStarter.Web, and DotNetStarter.Owin
 * DotNetStarter now requires an [ILocator implementation](https://bmcdavid.github.io/DotNetStarter/ilocator-setup.html) to run.
