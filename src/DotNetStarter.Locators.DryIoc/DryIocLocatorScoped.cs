namespace DotNetStarter.Locators
{
    using System;
    using System.Collections.Generic;
    using DotNetStarter.Abstractions;
    using DryIoc;

    /// <summary>
    /// Scoped DryIoc locator
    /// </summary>
    public sealed class DryIocLocatorScoped : ILocatorScoped, ILocatorCreateScope
    {
        private readonly IResolverContext _resolveContext;        
        private Action _onDispose;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="locator"></param>
        public DryIocLocatorScoped(IResolverContext context, ILocator locator)
        {
            _resolveContext = context;
            Parent = locator as ILocatorScoped;
        }

        /// <summary>
        /// Not Supported debug info
        /// </summary>
        public string DebugInfo => "not supported";

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// Creates a child scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope()
        {
            return new DryIocLocatorScoped(_resolveContext.OpenScope(), this);
        }

        /// <summary>
        /// Disposes scoped container
        /// </summary>
        public void Dispose()
        {
            _onDispose?.Invoke();
            _resolveContext.Dispose();
        }

        /// <summary>
        /// Gets a scoped instance of service Type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null)
        {
            return _resolveContext.Resolve(serviceType);
        }

        /// <summary>
        /// Gets a scoped instance T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null)
        {
            return _resolveContext.Resolve<T>();
        }

        /// <summary>
        /// Gets all scoped T instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _resolveContext.ResolveMany<T>();
        }

        /// <summary>
        /// Gets all scoped instances
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _resolveContext.ResolveMany(serviceType);
        }

        /// <summary>
        /// Action to perform on disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction)
        {
            _onDispose += disposeAction;
        }
    }
}