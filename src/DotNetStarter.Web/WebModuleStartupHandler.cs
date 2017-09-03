#if !NETSTANDARD1_3

namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System;
    using static ApplicationContext;   

    /// <summary>
    /// Initializes IHttpModules
    /// </summary>
    [Register(typeof(IWebModuleStartupHandler), LifeTime.Singleton)]
    public class WebModuleStartupHandler : IWebModuleStartupHandler
    {
        private ILocator _Locator;
        private IEnumerable<IStartupModule> _StartupModules;

        /// <summary>
        /// Deprecated constructor
        /// </summary>
        /// <param name="context"></param>
        [Obsolete]
        public WebModuleStartupHandler(IStartupContext context) : this(context.Locator, context.Locator.GetAll<IStartupModule>())
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="startupModules"></param>
        public WebModuleStartupHandler(ILocator locator, IEnumerable<IStartupModule> startupModules)
        {
            _Locator = locator;
            _StartupModules = startupModules;
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
            startupModules = startupModules ?? _StartupModules.OfType<IHttpModule>();

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
                var scopedLocator = _Locator.OpenScope();
                var scopedRegistry = scopedLocator as ILocatorRegistry;
                scopedRegistry?.Add(typeof(ILocator), scopedLocator); // override ILocator resolves to use scoped version
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
#endif