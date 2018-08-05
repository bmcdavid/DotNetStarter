namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using static ApplicationContext;

    /// <summary>
    /// Initializes IHttpModules
    /// </summary>
    [Registration(typeof(IWebModuleStartupHandler), Lifecycle.Singleton)]
    public class WebModuleStartupHandler : IWebModuleStartupHandler
    {
        private ILocatorScopedFactory _LocatorScopeFactory;
        private IEnumerable<IStartupModule> _StartupModules;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorScopeFactory"></param>
        /// <param name="locator"></param>
        public WebModuleStartupHandler(ILocatorScopedFactory locatorScopeFactory, ILocator locator)
        {
            _LocatorScopeFactory = locatorScopeFactory;
            _StartupModules = locator.GetAll<IStartupModule>(); // hack: needed to get correct sorting on some locators!
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