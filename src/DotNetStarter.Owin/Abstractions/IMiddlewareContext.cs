namespace DotNetStarter.Owin.Abstractions
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Provides access to OWIN environment
    /// </summary>
    public interface IMiddlewareContext
    {
        /// <summary>
        /// Gets a wrapper exposing request specific properties.
        /// </summary>
        /// <returns>A wrapper exposing request specific properties.</returns>
        IMiddlewareRequest Request { get; }

        /// <summary>
        /// Gets a wrapper exposing response specific properties.
        /// </summary>
        /// <returns>A wrapper exposing response specific properties.</returns>
        IMiddlewareResponse Response { get; }

        /// <summary>
        /// Gets the OWIN environment.
        /// </summary>
        /// <returns>The OWIN environment.</returns>
        IDictionary<string, object> Environment { get; }

        /// <summary>
        /// Gets or sets the host.TraceOutput environment value.
        /// </summary>
        /// <returns>The host.TraceOutput TextWriter.</returns>
        TextWriter TraceOutput { get; set; }

        /// <summary>
        /// Gets a value from the OWIN environment, or returns default(T) if not present.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value with the specified key or the default(T) if not present.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Sets the given key and value in the OWIN environment.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>This instance.</returns>
        IMiddlewareContext Set<T>(string key, T value);
    }
}