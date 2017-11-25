---
title: DotNetStarter - ASP.Net Core Configure Services
---
# DotNetStarter - ASP.Net Core Configure Services

Below are examples of how to configure the IServiceProvider in Configure Services for the supported locators.

## DotNetStarter

DotNetStarter can now create the service provider in the configure set. The only issue with using it is it selects the greediest constructor and not all services are wired up in that manner. Below is a simple example that works with all three of the maintained locators.

### Required Packages

* DotNetStarter.RegistrationAbstractions
* DotNetStarter.Abstractions
* DotNetStarter
* One of the following:
  * DotNetStarter.DryIoc
  * DotNetStarter.Structuremap
  * DotNetStarter.Locators.LightInject

```cs

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddViewLocalization();

            // the implementation has 2 greediest constructors with different arguments
            var loggerFactoryType = typeof(Microsoft.Extensions.Logging.ILoggerFactory);
            var loggerDescriptor = services.FirstOrDefault(x => x.ServiceType == loggerFactoryType);

            if (loggerDescriptor != null)
            {
                services.Remove(loggerDescriptor);
                services.Add(new ServiceDescriptor
                (
                    loggerFactoryType,
                    new Microsoft.Extensions.Logging.LoggerFactory()
                ));
            }

            // add this to provide missing argument for Microsoft.AspNetCore.Mvc.Internal.MvcRouteHandler
            services.Add(new ServiceDescriptor
            (
                typeof(IActionContextAccessor),
                typeof(ActionContextAccessor),
                ServiceLifetime.Singleton
            ));

            var provider = services.WithDotNetStarter(() => StartupDotNetStarter(Env));

            return provider;
        }

        public static void StartupDotNetStarter(IHostingEnvironment hostingEnvironment)
        {
            // Add the following lines in the Startup class constructor, for netcore assembly loading
            Func<IEnumerable<Assembly>> assemblyLoader = () =>
            {
                var runtimeId = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
                var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames(Microsoft.Extensions.DependencyModel.DependencyContext.Default,
                    runtimeId);

                return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
            };

            var hostingEnv = new DotNetStarter.StartupEnvironment(hostingEnvironment.EnvironmentName, hostingEnvironment.ContentRootPath);
            var filteredAssemblies = DotNetStarter.ApplicationContext.GetScannableAssemblies(assemblies: assemblyLoader());

            // Invoke DotNetStarter Startup
            ApplicationContext.Startup(assemblies: filteredAssemblies.Union(new Assembly[] { typeof(Example.Startup).Assembly }), environment: hostingEnv);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
```

## DryIoc Locator

### Required Nuget packages

* DotNetStarter
* DotNetStarter.DryIoC
* DryIoc.Microsoft.DependencyInjection

```cs
using DotNetStarter;
using DotNetStarter.Abstractions;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Example
{
    public class CompositeRoot { } // needed for DryIoc extension

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // fix for known assembly loading issue
            Func<IEnumerable<Assembly>> assemblyLoader = () =>
            {
                var runtimeId = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
                var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames(Microsoft.Extensions.DependencyModel.DependencyContext.Default, runtimeId);

                return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
            };

            DotNetStarter.ApplicationContext.Startup(assemblies: assemblyLoader());

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Configure services as needed
            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

            // Add framework services.
            services.AddMvc();

            // get current configured locator
            var registry = Context.Default.Locator as ILocatorRegistry;

            // get internal container to execute extension from DryIoc.Microsoft.DependencyInjection
            var tempContainer = (registry.InternalContainer as DryIoc.IContainer).WithDependencyInjectionAdapter(services);

            // update current locator with newly configured container
            (registry as ILocatorSetContainer)?.SetContainer(tempContainer);

            // Return an IServiceProvider from DryIoc
            return tempContainer.ConfigureServiceProvider<CompositeRoot>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // omitted for brevity
        }
    }
}

```

## Structuremap Locator

### Required Nuget Packages
* DotNetStarter
* DotNetStarter.Structuremap
* Structuremap.Microsoft.DependencyInjection

```cs
using DotNetStarter;
using DotNetStarter.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using System;

namespace Example
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // fix for known issue
            Func<IEnumerable<Assembly>> assemblyLoader = () =>
            {
                var runtimeId = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
                var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames(Microsoft.Extensions.DependencyModel.DependencyContext.Default, runtimeId);

                return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
            };

            DotNetStarter.ApplicationContext.Startup(assemblies: assemblyLoader());

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Configure services as needed
            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

            // Add framework services.
            services.AddMvc();

            // get current configured locator
            var registry = Context.Default.Locator as ILocatorRegistry;

            // get internal container for additional
            var tempContainer = (registry.InternalContainer as StructureMap.IContainer);

            tempContainer?.Configure(config =>
            {
                config.Populate(services); // add services
            });

            // update current locator with newly configured container
            (registry as ILocatorSetContainer)?.SetContainer(tempContainer);

            return tempContainer.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           // omitted for brevity
        }
    }
}

```
