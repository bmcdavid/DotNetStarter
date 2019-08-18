namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using static DotNetStarter.Abstractions.Keys;

    /// <summary>
    /// Initializes IHttpModules
    /// </summary>
    [Registration(typeof(IWebModuleStartupHandler), Lifecycle.Singleton)]
    public class WebModuleStartupHandler : IWebModuleStartupHandler
    {
        private readonly ILocatorScopedFactory _locatorScopeFactory;
        private readonly IEnumerable<IStartupModule> _startupModules;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorScopeFactory"></param>
        /// <param name="locator"></param>
        public WebModuleStartupHandler(ILocatorScopedFactory locatorScopeFactory, ILocator locator)
        {
            _locatorScopeFactory = locatorScopeFactory;
            _startupModules = locator.GetAll<IStartupModule>(); // hack: needed to get correct sorting on some locators!
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
            startupModules = startupModules ?? _startupModules.OfType<IHttpModule>();

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
                var scopedLocator = _locatorScopeFactory.CreateScope();
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