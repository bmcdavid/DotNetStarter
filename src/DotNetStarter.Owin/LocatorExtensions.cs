﻿namespace DotNetStarter.Owin
{
    using Abstractions;
    using DotNetStarter.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using static DotNetStarter.ApplicationContext;

#if NET45
    using global::Owin;
    using System.Linq;
    using System.Collections;
#endif

    /// <summary>
    /// Use Extensions for Applications
    /// </summary>
    public static class LocatorExtensions
    {
        /// <summary>
        /// Retrieves scope container stored in OWIN context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scoped container.</returns>
        public static IServiceProvider GetScopedServiceProvider(this IDictionary<string, object> context)
        {
            return Get<IServiceProvider>(context, ScopedProviderKeyInContext, null);
        }

        /// <summary>
        /// Retrieves scope container stored in OWIN context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scoped container.</returns>
        public static ILocator GetScopedLocator(this IDictionary<string, object> context)
        {
            return Get<ILocator>(context, ScopedLocatorKeyInContext, null);
        }

        /// <summary>
        /// Gets T from given items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary<string, object> items, string key, T fallback)
        {
            object o = null;
            items?.TryGetValue(key, out o);

            return o == null ? fallback : (T)o;
        }

        /// <summary>
        /// Sets item in OWIN context
        /// </summary>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<string, object> Set(this IDictionary<string, object> items, string key, object value)
        {
            if (items == null || key == null)
                throw new ArgumentNullException($"{nameof(items)} or {nameof(key)} cannot be null!");

            items[key] = value;

            return items;
        }

        /// <summary>
        /// Tries to get already scoped locator from HttpContextBase.Items dictionary
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="rootLocator"></param>
        /// <returns></returns>
        internal static ILocator TryGetHttpScopedLocator(IDictionary<string, object> environment, ILocator rootLocator)
        {
            var reflectionHelper = rootLocator.Get<IReflectionHelper>();
            var httpContext = environment.Get<IServiceProvider>("System.Web.HttpContextBase", null);
            var httpContextItemProperty = httpContext == null ? null :
                    reflectionHelper.GetProperties(httpContext.GetType()).FirstOrDefault(x => string.CompareOrdinal(x.Name, "Items") == 0);

            var contextItemDict = httpContextItemProperty?.GetValue(httpContext) as IDictionary;

            if (contextItemDict != null)
            {
                return contextItemDict[ScopedLocatorKeyInContext] as ILocator;
            }

            return null;
        }

#if NET45
        /// <summary>
        /// Opens scope for OWIN pipeline as an IServiceProvider, scopename and scopecontext are for DryIoc containers.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="locator"></param>
        /// <param name="scopeName"></param>
        /// <param name="addToScope"></param>
        /// <param name="scopeContext"></param>
        public static void UseScopedLocator(this IAppBuilder app, ILocator locator, Action<ILocatorRegistry> addToScope = null, object scopeName = null, object scopeContext = null)
        {
            app.Use(new Func<AppFunc, AppFunc>(next => (async context =>
            {
                var scoped = TryGetHttpScopedLocator(context, locator);
                var hasScopedLocator = scoped != null;
                scoped = scoped ?? locator.OpenScope(scopeName, scopeContext); // create a new scope if not found.

                //// register items
                var registry = scoped as ILocatorRegistry;
                registry?.Add(typeof(IDictionary<string, object>), context);
                registry?.Add(typeof(IMiddlewareContext), new MiddlewareContext(context));
                addToScope?.Invoke(registry);

                var serviceProvider = new ServiceProvider(scoped);
                context[ScopedProviderKeyInContext] = serviceProvider;
                context[ScopedLocatorKeyInContext] = scoped;

                // perform remaining tasks
                await next.Invoke(context);

                if (!hasScopedLocator)
                    scoped?.Dispose();

            })));
        }
#endif
        internal sealed class MiddlewareWrapper<TServiceMiddleware> where TServiceMiddleware : IOwinMiddleware
        {
            public MiddlewareWrapper(AppFunc next)
            {
                Next = next;
            }

            public AppFunc Next { get; }

            public Task Invoke(IDictionary<string, object> context)
            {
                var scopedProvider = context.GetScopedServiceProvider();

                if (scopedProvider == null)
                    throw new NullReferenceException($"Cannot get {typeof(IServiceProvider).FullName} from current context for key {ScopedProviderKeyInContext}!");

                var middleware = scopedProvider.GetService(typeof(Func<AppFunc, TServiceMiddleware>)) as Func<AppFunc, TServiceMiddleware>;

                if (middleware == null)
                    return Next.Invoke(context);

                return middleware(Next).Invoke(context);
            }
        }
    }
}
