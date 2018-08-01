namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Provides access to container add services to the ILocator
    /// </summary>
    [CriticalComponent]
    public interface ILocatorRegistry
    {
        /// <summary>
        /// Adds service to container
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifecycle = Lifecycle.Transient);

        /// <summary>
        /// Adds service to container
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory">Important: Given ILocator can only get Singleton and Transient (with no scoped parameters), use with caution!</param>
        /// <param name="lifecycle"></param>        
        void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifecycle);

        /// <summary>
        /// Adds instance to container, the lifetime is set to Singleton
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>        
        void Add(Type serviceType, object serviceInstance);

        /// <summary>
        /// Adds service to container
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="key"></param>
        /// <param name="lifecycle"></param>
        void Add<TService, TImpl>(string key = null, Lifecycle lifecycle = Lifecycle.Transient) where TImpl : TService;

        /// <summary>
        /// Gets underlying container
        /// </summary>
        object InternalContainer { get; }

        /// <summary>
        /// Allows ILocatorRegistry to perform any final tasks after container setup complete is invoked
        /// </summary>
        void Verify();
    }
}
