namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System;
    using static Context;

    // todo: move to its own file

    /// <summary>
    /// HttpContext extensions for System.Web
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets a scoped locator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILocator GetScopedLocator(this HttpContext context)
        {
            return context?.Items[ScopedLocatorKeyInContext] as ILocator;
        }

        /// <summary>
        /// Gets a scoped locator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILocator GetScopedLocator(this HttpContextBase context)
        {
            return context?.Items[ScopedLocatorKeyInContext] as ILocator;
        }
    }

    /// <summary>
    /// Initializes IHttpModules
    /// </summary>
    [Register(typeof(IWebModuleStartupHandler), LifeTime.Singleton)]
    public class WebModuleStartupHandler : IWebModuleStartupHandler
    {
        private IStartupContext _StartupContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public WebModuleStartupHandler(IStartupContext context)
        {
            _StartupContext = context;
        }
        
        /// <summary>
        /// Default is true.
        /// </summary>
        /// <returns></returns>
        public virtual bool StartupEnabled() => true;

        /// <summary>
        /// Initializes modules
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="startupModules"></param>
        public virtual void Startup(HttpApplication applicationContext, IEnumerable<IHttpModule> startupModules)
        {
            startupModules = startupModules ?? _StartupContext.Locator.GetAll<IStartupModule>().OfType<IHttpModule>();

            foreach(var module in startupModules)
            {
                module.Init(applicationContext);
            }
        }

        /// <summary>
        /// Default is true
        /// </summary>
        /// <returns></returns>
        public virtual bool ScopeEnabled() => true;

        /// <summary>
        /// Opens and closes a scoped locator
        /// </summary>
        /// <param name="applicationContext"></param>
        public virtual void OpenLocatorScope(HttpApplication applicationContext)
        {
            applicationContext.BeginRequest += (sender, args) =>
            {
                var x = sender as HttpApplication;
                var context = x.Context;

                var scopedLocator = Default.Locator.OpenScope();
                var scopedRegistry = scopedLocator as ILocatorRegistry;
                scopedRegistry?.Add(typeof(HttpContextBase), new HttpContextWrapper(context));
                scopedRegistry?.Add<IServiceProvider, ServiceProvider>(lifetime: LifeTime.Scoped);

                context.Items.Add(ScopedLocatorKeyInContext, scopedLocator);
            };

            applicationContext.EndRequest += (sender, args) =>
            {
                var x = sender as HttpApplication;
                var context = x.Context;

                var scope = context.Items[ScopedLocatorKeyInContext] as ILocator;
                scope?.Dispose();
            };
        }
    }
}