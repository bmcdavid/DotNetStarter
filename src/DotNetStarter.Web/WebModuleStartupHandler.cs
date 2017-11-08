#if !NETSTANDARD1_3

namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using static ApplicationContext;

    /// <summary>
    /// Initializes IHttpModules
    /// </summary>
    [Register(typeof(IWebModuleStartupHandler), LifeTime.Singleton)]
    public class WebModuleStartupHandler : IWebModuleStartupHandler
    {
        private ILocatorScopedFactory _LocatorScopeFactory;
        private IEnumerable<IStartupModule> _StartupModules;

        /// <summary>
        /// Deprecated constructor
        /// </summary>
        /// <param name="context"></param>
        [Obsolete]
        public WebModuleStartupHandler(IStartupContext context) : this(context.Locator.Get<ILocatorScopedFactory>(), context.Locator.GetAll<IStartupModule>())
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorScopeFactory"></param>
        /// <param name="startupModules"></param>        
        public WebModuleStartupHandler(ILocatorScopedFactory locatorScopeFactory, IEnumerable<IStartupModule> startupModules)
        {
            _LocatorScopeFactory = locatorScopeFactory;
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

            foreach (var module in startupModules)
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
                var scopedLocator = _LocatorScopeFactory.CreateScope();
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