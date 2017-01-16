# DotNetStarter.Web Read Me

This package supports native .netframeworks 3.5+ for System.Web and is intended for IIS compatible sites.

For netstandard1.3+ there are a few extensions at this point.

IHttpModule(s) startup by inheriting IWebModule or IHttpModule and IStartupModule. These modules also require the [StartupModuleAttribute].

Modules will be ordered by their dependencies setup by the [StartupModuleAttribute].

## Important note for ASP.NET 3.5 applications only

This package doesn't include an web.config transfrom, the handler must be manually added to the system.webserver section. Below is a sample
```
<system.webServer>
    <modules>
        <add name="DotNetStarter.Web.WebModuleStartup" type="DotNetStarter.Web.WebModuleStartup, DotNetStarter.Web" preCondition="managedHandler"/>
	</modules>
</system.webServer>
```