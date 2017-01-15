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
    using Microsoft.Extensions.DependencyInjection;
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
        /// Dictionary Key to retrive scoped IServiceProvider
        /// </summary>
        public static readonly string ScopedKeyInContext = typeof(LocatorExtensions).FullName;

        /// <summary>
        /// Retrieves scope container stored in OWIN context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scoped container.</returns>
        public static IServiceProvider GetServiceProvider(this IDictionary<string, object> context)
        {
            object scoped = null;
            context?.TryGetValue(ScopedKeyInContext, out scoped);

            return scoped as IServiceProvider;
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
                //todo: figure out how to handle if scope opened in IHttpModule
                using (var scope = locator.OpenScope(scopeName, scopeContext))
                {
                    var registry = scope as ILocatorRegistry;
                    registry?.Add(typeof(IDictionary<string, object>), context);
                    registry?.Add(typeof(IMiddlewareContext), new MiddlewareContext(context));
                    addToScope?.Invoke(registry);

                    var serviceProvider = new ServiceProvider(scope);
                    context[ScopedKeyInContext] = serviceProvider;

                    await next.Invoke(context);
                }
            })));
        }
        
#elif NETSTANDARD
        /// <summary>
        /// Stores IServiceProvider in builder pipeline.
        /// </summary>
        /// <param name="app"></param>
        public static void UseScopedLocator(this IApplicationBuilder app)
        {
            app.Use(new Func<RequestDelegate, RequestDelegate>(next => (async context =>
            {
                // netcore uses IScopeFactory that wraps all middleware calls, so a scope.Open isn't needed 
                context.Items[ScopedKeyInContext] = context.RequestServices; //context.RequestServices is already opened scope

                await next(context);
            })));
        }

        /// <summary>
        /// Adds service collection items to locator
        /// </summary>
        /// <param name="services"></param>
        /// <param name="locator"></param>
        public static IServiceProvider AddServicesToLocator(this IServiceCollection services, ILocatorRegistry locator)
        {
            if (locator == null)
                throw new ArgumentNullException();

            locator.Add<IServiceProvider, ServiceProvider>(lifetime: LifeTime.Scoped);

            // Scope factory should be scoped itself to enable nested scopes creation
            locator.Add<IServiceScopeFactory, ServiceScopeFactory>(lifetime: LifeTime.Scoped);
            
            // map .net services to locator
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                var lifetime = ConvertLifeTime(service.Lifetime);                

                if (service.ImplementationType != null)
                {
                    locator.Add(service.ServiceType, service.ImplementationType, lifeTime: lifetime);
                }
                else if (service.ImplementationFactory != null)
                {
                    locator.Add(service.ServiceType, l => service.ImplementationFactory(l.Get<IServiceProvider>()), lifetime);
                }
                else
                {
                    locator.Add(service.ServiceType, service.ImplementationInstance);
                }
            }

            return locator.Get<IServiceProvider>();
        }

        private static LifeTime ConvertLifeTime(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    return LifeTime.Scoped;
                case ServiceLifetime.Singleton:
                    return LifeTime.Singleton;
                case ServiceLifetime.Transient:
                    return LifeTime.Transient;
            }

            return LifeTime.Transient;
        }

        /// <summary>
        /// Retrieves scope container stored in HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceProvider GetServiceProvider(this HttpContext context)
        {
            object scoped = null;
            context?.Items?.TryGetValue(ScopedKeyInContext, out scoped);

            return scoped as IServiceProvider;
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
                var scopedProvider = context.GetServiceProvider();

                if (scopedProvider == null)
                    throw new NullReferenceException($"Cannot get {typeof(IServiceProvider).FullName} from current context for key {ScopedKeyInContext}!");

                var middleware = scopedProvider.GetService(typeof(Func<AppFunc, TServiceMiddleware>)) as Func<AppFunc, TServiceMiddleware>;

                if (middleware == null)
                    return Next.Invoke(context);

                return middleware(Next).Invoke(context);
            }
        }
    }
}
