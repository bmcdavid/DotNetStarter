# DotNetStarter.Extensions.Episerver Read Me

This package requires DotNetStarter.Extensions.Mvc to setup a controller builder.

A static action 'ContainerSet' has been added to DotNetStarter.Extensions.Episerver.EpiserverLocatorSetup to allow developers a place to 
invoke a customized DotNetStarter.ApplicationContext.Startup() in a System.Web.PreApplicationStartMethod assembly attribute.

**Important:** If any Episerver initalization modules use DotNetStarter,
	add a [ModuleDependency(typeof(DotNetStarter.Extensions.Episerver.EpiserverLocatorSetup))] module dependency attribute to ensure its started.