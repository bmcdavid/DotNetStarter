---
title: DotNetStarter - Customizing the object factory
---
# DotNetStarter - Customizing the object factory

The object factory is responsible for creating objects before the Ioc/DI container is setup. 
Implementations must use the IStartupObjectFactory interface.

 ## Startup Assignment
 To create a custom object factory, an implementation of IStartupObjectFactory can be passed to **DotNetStarter.ApplicationContext.Startup()**