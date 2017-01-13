namespace DotNetStarter.Owin
{
    using Abstractions;
    using DotNetStarter.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

#if NETSTANDARD
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
#elif NET45
    using global::Owin;
    using System.Linq;
#endif

    /// <summary>
    /// Use Extensions for Applications
    /// </summary>
    public static class LocatorExtensions
    {
        /// <summary>
        /// Dictionary Key to retrive scoped container
        /// </summary>
        public static readonly string ScopedLocatorKeyInContext = typeof(LocatorExtensions).FullName;

        /// <summary>
        /// Retrieves scope container stored in OWIN context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scoped container.</returns>
        public static ILocator GetScopedLocator(this IDictionary<string, object> context)
        {
            object scopedLocator = null;
            context?.TryGetValue(ScopedLocatorKeyInContext, out scopedLocator);

            return scopedLocator as ILocator;
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

#if NET45
        /// <summary>
        /// Opens scope for OWIN pipeline, default scopename and scopecontext are DryIoc specific.
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
                using (var scope = locator.OpenScope(scopeName, scopeContext))
                {
                    var registry = scope as ILocatorRegistry;
                    registry?.Add(typeof(IDictionary<string, object>), context);
                    //registry?.Add(typeof(IMiddlewareContext), new MiddlewareContext(context));
                    addToScope?.Invoke(registry);
                    context[ScopedLocatorKeyInContext] = scope;

                    await next.Invoke(context);
                }
            })));
        }
        
#elif NETSTANDARD
        /// <summary>
        /// Opens scope for IApplicationBuilder pipeline and injects the HttpContext for scoped locator.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="locator"></param>
        /// <param name="scopeName"></param>
        /// <param name="addToScope"></param>
        /// <param name="scopeContext"></param>
        public static void UseScopedLocator(this IApplicationBuilder app, ILocator locator, Action<ILocatorRegistry> addToScope = null, object scopeName = null, object scopeContext = null)
        {
            app.Use(new Func<RequestDelegate, RequestDelegate>(next => (async context =>
            {
                // wraps all following items in scoped locator
                using (var scope = locator.OpenScope(scopeName, scopeContext))
                {
                    var registry = scope as ILocatorRegistry;
                    registry?.Add(typeof(HttpContext), context);
                    addToScope?.Invoke(registry);
                    context.Set(ScopedLocatorKeyInContext, scope);

                    await next(context);
                }
            })));
        }

        /// <summary>
        /// Retrieves scope container stored in HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILocator GetScopedLocator(this HttpContext context)
        {
            object scopedLocator = null;
            context?.Items?.TryGetValue(ScopedLocatorKeyInContext, out scopedLocator);

            return scopedLocator as ILocator;
        }

        /// <summary>
        /// Gets T from given HttpContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static T Get<T>(this HttpContext context, string key, T fallback)
        {
            object o = null;
            context?.Items?.TryGetValue(key, out o);

            return o == null ? fallback : (T)o;
        }

        /// <summary>
        /// Sets item in HttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HttpContext Set(this HttpContext context, object key, object value)
        {
            if (context?.Items == null || key == null)
                throw new ArgumentNullException($"{nameof(context)} or {nameof(key)} cannot be null!");

            context.Items[key] = value;

            return context;
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
                var scopedContainer = context.GetScopedLocator();

                if (scopedContainer == null)
                    throw new NullReferenceException($"Cannot get {typeof(ILocator).FullName} from current context for key {ScopedLocatorKeyInContext}!");

                var middleware = scopedContainer.Get<Func<AppFunc, TServiceMiddleware>>();

                if (middleware == null)
                    return Next.Invoke(context);

                return middleware(Next).Invoke(context);
            }
        }
    }

    /// <summary>
    /// Class that typically allows for additonal IRegistrator entries, but those should funnel through container registrations
    /// </summary>
    internal class CompositionRoot { }
}
