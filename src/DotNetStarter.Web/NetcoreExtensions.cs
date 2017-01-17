using DotNetStarter.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using static DotNetStarter.Context;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Netcore Extensions
    /// </summary>
    public static class NetcoreExtensions
    {
        /// <summary>
        /// Stores IServiceProvider and ILocator in HttpContext.Items
        /// </summary>
        /// <param name="app"></param>
        public static void UseScopedLocator(this IApplicationBuilder app)
        {
            app.Use(new Func<RequestDelegate, RequestDelegate>(next => (async context =>
            {
                // netcore uses IScopeFactory that wraps all middleware calls, so a scope.Open isn't needed 
                context.Items[ScopedProviderKeyInContext] = context.RequestServices; //context.RequestServices is already opened scope
                context.Items[ScopedLocatorKeyInContext] = context.RequestServices.GetService(typeof(ILocator));

                await next(context);
            })));
        }

        /// <summary>
        /// Retrieves scope container stored in HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceProvider GetScopedServiceProvider(this HttpContext context)
        {
            return Get<IServiceProvider>(context, ScopedProviderKeyInContext, null);
        }

        /// <summary>
        /// Retrieves scope container stored in HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILocator GetScopedLocator(this HttpContext context)
        {
            return Get<ILocator>(context, ScopedLocatorKeyInContext, null);
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
    }
}
