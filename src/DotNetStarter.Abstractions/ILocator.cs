namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines service location operations
    /// </summary>
    public interface ILocator : IDisposable
    {
        /// <summary>
        /// Gets underlying container
        /// </summary>
        object InternalContainer { get; }

        //todo: v2 change this signature to no parameters, and returns an ILocatorRegistry

        /// <summary>
        /// Creates a scoped locator
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        ILocator OpenScope(object scopeName = null, object scopeContext = null);

        /// <summary>
        /// Debug information about container
        /// </summary>
        string DebugInfo { get; }

        /// <summary>
        /// Build up object from container
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool BuildUp(object target);

        /// <summary>
        /// Gets item from container
        /// <para>Important: Throwing exceptions instead of returning null when a type cannot resolve in the container is strongly encouraged. </para>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(Type serviceType, string key = null);

        /// <summary>
        /// Get item from container
        /// <para>Important: Throwing exceptions instead of returning null when a type cannot resolve in the container is strongly encouraged. </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key = null);

        /// <summary>
        /// Get all items from container
        /// <para>Important: Throwing exceptions instead of returning null when a type cannot resolve in the container is strongly encouraged. </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(string key = null);

        /// <summary>
        /// Gets all services of container
        /// <para>Important: Throwing exceptions instead of returning null when a type cannot resolve in the container is strongly encouraged. </para>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IEnumerable<object> GetAll(Type serviceType, string key = null);
    }
}
