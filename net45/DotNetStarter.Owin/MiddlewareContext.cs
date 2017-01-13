namespace DotNetStarter.Owin
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 
    /// </summary>
    public class MiddlewareContext : IMiddlewareContext
    {
        /// <summary>
        /// Create a new context with only request and response header collections.
        /// </summary>
        public MiddlewareContext()
        {
            IDictionary<string, object> environment = new Dictionary<string, object>(StringComparer.Ordinal);
            environment[MiddlewareOwinConstants.RequestHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            environment[MiddlewareOwinConstants.ResponseHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            Environment = environment;
            Request = new MiddlewareRequest(environment);
            Response = new MiddlewareResponse(environment);
        }

        /// <summary>
        /// Create a new wrapper.
        /// </summary>
        /// <param name="environment">OWIN environment dictionary which stores state information about the request, response and relevant server state.</param>
        public MiddlewareContext(IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            Environment = environment;
            Request = new MiddlewareRequest(environment);
            Response = new MiddlewareResponse(environment);
        }

        /// <summary>
        /// Gets a wrapper exposing request specific properties.
        /// </summary>
        /// <returns>A wrapper exposing request specific properties.</returns>
        public virtual IMiddlewareRequest Request { get; }

        /// <summary>
        /// Gets a wrapper exposing response specific properties.
        /// </summary>
        /// <returns>A wrapper exposing response specific properties.</returns>
        public virtual IMiddlewareResponse Response { get; }

        /// <summary>
        /// Gets the OWIN environment.
        /// </summary>
        /// <returns>The OWIN environment.</returns>
        public virtual IDictionary<string, object> Environment { get; }

        /// <summary>
        /// Gets or sets the host.TraceOutput environment value.
        /// </summary>
        /// <returns>The host.TraceOutput TextWriter.</returns>
        public virtual TextWriter TraceOutput
        {
            get { return Get<TextWriter>(MiddlewareOwinConstants.CommonKeys.TraceOutput); }
            set { Set<TextWriter>(MiddlewareOwinConstants.CommonKeys.TraceOutput, value); }
        }

        /// <summary>
        /// Gets a value from the OWIN environment, or returns default(T) if not present.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value with the specified key or the default(T) if not present.</returns>
        public virtual T Get<T>(string key)
        {
            object value;

            return Environment.TryGetValue(key, out value) ? (T)value : default(T);
        }

        /// <summary>
        /// Sets the given key and value in the OWIN environment.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>This instance.</returns>
        public virtual IMiddlewareContext Set<T>(string key, T value)
        {
            Environment[key] = value;

            return this;
        }
    }
}
