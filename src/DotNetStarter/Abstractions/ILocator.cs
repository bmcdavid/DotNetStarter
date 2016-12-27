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
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(Type serviceType, string key = null);

        /// <summary>
        /// Get item from container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key = null);

        /// <summary>
        /// Get all items from container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(string key = null);

        /// <summary>
        /// Gets all services of container
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IEnumerable<object> GetAll(Type serviceType, string key = null);
    }
}
