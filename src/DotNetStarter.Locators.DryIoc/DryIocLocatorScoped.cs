namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DryIoc;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Scoped DryIoc locator
    /// </summary>
    public sealed class DryIocLocatorScoped : ILocatorScoped, ILocatorWithCreateScope
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
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// Creates a child scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope() => new DryIocLocatorScoped(_resolveContext.OpenScope(), this);

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
        public object Get(Type serviceType, string key = null) => _resolveContext.Resolve(serviceType);

        /// <summary>
        /// Gets a scoped instance T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => _resolveContext.Resolve<T>();

        /// <summary>
        /// Gets all scoped T instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null) => _resolveContext.ResolveMany<T>();

        /// <summary>
        /// Gets all scoped instances
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => _resolveContext.ResolveMany(serviceType);

        /// <summary>
        /// Action to perform on disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction) => _onDispose += disposeAction;
    }
}