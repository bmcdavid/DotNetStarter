namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines container resolving operations to a conforming container defined at <see cref="!:http://blog.ploeh.dk/2014/05/19/conforming-container/">here</see>
    /// <para>Important: The best practice is to avoid getting services directly from the locator, in favor of constructor injection.
    /// If a new transient is needed in an implementation, inject a Func&lt;T> where T is a registered transient service.</para>
    /// </summary>
    public interface ILocator : IDisposable
    {
        /// <summary>
        /// Gets underlying container
        /// </summary>
        object InternalContainer { get; }

        //todo: v2, remove OpenScope from ILocator

        /// <summary>
        /// Creates a scoped locator
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="scopeContext"></param>
        /// <returns></returns>
        [Obsolete("OpenScope will be removed from ILocator in version 2.")]
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
