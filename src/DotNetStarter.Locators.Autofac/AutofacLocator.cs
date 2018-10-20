using Autofac;
using Autofac.Builder;
using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Autofac Locator
    /// </summary>
    public class AutofacLocator : ILocator, ILocatorWithCreateScope, IServiceProvider
    {
        private readonly ContainerBuilder _containerBuilder;
        private readonly ContainerBuildOptions _options;
        private readonly Func<IContainer> _containerFactory;
        private IContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="options"></param>
        public AutofacLocator(ContainerBuilder containerBuilder, ContainerBuildOptions options)
        {
            _containerBuilder = containerBuilder;
            _options = options;
        }

        /// <summary>
        /// Constructor where container is provided
        /// </summary>
        /// <param name="containerFactory"></param>
        public AutofacLocator(Func<IContainer> containerFactory)
        {
            _containerFactory = containerFactory;
        }

        /// <summary>
        /// Creates a scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope() => new AutofacScopedLocator(ResolveContainer().BeginLifetimeScope(), this);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => ResolveContainer().Dispose();

        /// <summary>
        /// Gets a service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null) => ResolveContainer().Resolve(serviceType);

        /// <summary>
        /// Gets a service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null) => ResolveContainer().Resolve<T>();

        /// <summary>
        /// Gets all services for type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null) => ResolveContainer().Resolve<IEnumerable<T>>();

        /// <summary>
        /// Gets all services for type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null) => ResolveContainer().Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;

        private IContainer ResolveContainer()
        {
            if (_container != null) { return _container; }

            return _container = _containerFactory?.Invoke() ?? _containerBuilder.Build(_options);
        }

        /// <summary>
        /// IServiceProvider.GetService
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType) => ResolveContainer().Resolve(serviceType);
    }
}