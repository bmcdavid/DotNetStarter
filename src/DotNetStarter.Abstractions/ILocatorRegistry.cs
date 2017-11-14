namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Provides access to container setup tasks, such as add, remove, contains
    /// </summary>
    public interface ILocatorRegistry : ILocator
    {
        /// <summary>
        /// Adds service to container
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceImplementation"></param>
        /// <param name="key"></param>
        /// <param name="lifeTime"></param>
        /// <param name="constructorType"></param>
        void Add(Type serviceType, Type serviceImplementation, string key = null, LifeTime lifeTime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest);

        /// <summary>
        /// Adds service to container
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory">Important: Given ILocator can only get Singleton and Transient (with no scoped parameters), use with caution!</param>
        /// <param name="lifeTime"></param>        
        void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime);

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
        /// <param name="lifetime"></param>
        /// <param name="constructorType"></param>
        void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest) where TImpl : TService;
    }
}
