namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines container resolving operations to a conforming container defined at <see cref="!:http://blog.ploeh.dk/2014/05/19/conforming-container/">here</see>
    /// <para>Important: The best practice is to avoid getting services directly from the locator, in favor of constructor injection.
    /// If a new transient is needed in an implementation, inject a Func&lt;T> where T is a registered transient service.</para>
    /// </summary>
    [CriticalComponent]
    public interface ILocator : IServiceProvider, IDisposable
    {
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