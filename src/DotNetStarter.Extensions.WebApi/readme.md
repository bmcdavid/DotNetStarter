# DotNetStarter.Extensions.WebApi Read Me

To enable, please add the following line to your GlobalConfiguration.Configure(Register) configuration callback.


```cs
config.DependencyResolver = new WebApiDependencyResolver(DotNetStarter.ApplicationContext.Default.Locator);
```