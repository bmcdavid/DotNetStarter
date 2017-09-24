#if !NETSTANDARD1_3

namespace DotNetStarter.Web
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Web.Internal.Features;
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
        private ILocator _Locator;
        private IEnumerable<IStartupModule> _StartupModules;
        private readonly ExperimentalScopedLocator _ExperimentalScopedLocator;

        /// <summary>
        /// Deprecated constructor
        /// </summary>
        /// <param name="context"></param>
        [Obsolete]
        public WebModuleStartupHandler(IStartupContext context) : this(context.Locator, context.Locator.GetAll<IStartupModule>(), context.Locator.Get<ExperimentalScopedLocator>())
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="startupModules"></param>
        /// <param name="experimentalScopedLocator"></param>
        public WebModuleStartupHandler(ILocator locator, IEnumerable<IStartupModule> startupModules, ExperimentalScopedLocator experimentalScopedLocator)
        {
            _Locator = locator;
            _StartupModules = startupModules;
            _ExperimentalScopedLocator = experimentalScopedLocator;
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

                // todo: v2, temporary until feature is complete and old scope removed
                if (_ExperimentalScopedLocator?.Enabled == true)
                {
                    var scopeCreator = _Locator as ILocatorCreateScope;

                    if (scopeCreator == null)
                    {
                        throw new Exception($"New scoped locator is enabled, but {_Locator.GetType().FullName} doesn't implement {typeof(ILocatorCreateScope).FullName}!");
                    }

                    var scopedLocator = scopeCreator.CreateScope(HttpRequestScopeKind.HttpRequest);
                    context.Items.Add(ScopedLocatorKeyInContext, scopedLocator);
                }
                else
                {
                    var scopedLocator = _Locator.OpenScope();
                    var scopedRegistry = scopedLocator as ILocatorRegistry;
                    scopedRegistry?.Add(typeof(ILocator), scopedLocator); // override ILocator resolves to use scoped version

                    // below now done in WebConfiguration.cs
                    //scopedRegistry?.Add(typeof(HttpContextBase), new HttpContextWrapper(context));
                    //scopedRegistry?.Add<IServiceProvider, ServiceProvider>(lifetime: LifeTime.Scoped);
                    context.Items.Add(ScopedLocatorKeyInContext, scopedLocator);
                }

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