namespace DotNetStarter.Owin
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// Context Request
    /// </summary>
    public class MiddlewareRequest : IMiddlewareRequest
    {
        /// <summary>
        /// Create a new context with only request and response header collections.
        /// </summary>
        public MiddlewareRequest()
        {
            IDictionary<string, object> environment = new Dictionary<string, object>(StringComparer.Ordinal);
            environment[MiddlewareOwinConstants.RequestHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            environment[MiddlewareOwinConstants.ResponseHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            Environment = environment;
        }

        /// <summary>
        /// Create a new environment wrapper exposing request properties.
        /// </summary>
        /// <param name="environment">OWIN environment dictionary which stores state information about the request, response and relevant server state.</param>
        public MiddlewareRequest(IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            Environment = environment;
        }

        /// <summary>
        /// Gets the OWIN environment.
        /// </summary>
        /// <returns>The OWIN environment.</returns>
        public virtual IDictionary<string, object> Environment { get; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <returns>The request context.</returns>
        public virtual IMiddlewareContext Context => null;

        /// <summary>
        /// Gets or set the HTTP method.
        /// </summary>
        /// <returns>The HTTP method.</returns>
        public virtual string Method
        {
            get { return Get<string>(MiddlewareOwinConstants.RequestMethod); }
            set { Set(MiddlewareOwinConstants.RequestMethod, value); }
        }

        /// <summary>
        /// Gets or set the HTTP request scheme from owin.RequestScheme.
        /// </summary>
        /// <returns>The HTTP request scheme from owin.RequestScheme.</returns>
        public virtual string Scheme
        {
            get { return Get<string>(MiddlewareOwinConstants.RequestScheme); }
            set { Set(MiddlewareOwinConstants.RequestScheme, value); }
        }

        /// <summary>
        /// Returns true if the owin.RequestScheme is https.
        /// </summary>
        /// <returns>true if this request is using https; otherwise, false.</returns>
        public virtual bool IsSecure => string.Equals(Scheme, MiddlewareOwinConstants.Https, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or set the Host header. May include the port.
        /// </summary>
        /// <return>The Host header.</return>
        public virtual string Host
        {
            get { return MiddlewareHelpers.GetHost(this); }
            set { MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.Host, value); }
        }

        /// <summary>
        /// Gets or set the owin.RequestPathBase.
        /// </summary>
        /// <returns>The owin.RequestPathBase.</returns>
        public virtual string PathBase
        {
            get { return Get<string>(MiddlewareOwinConstants.RequestPathBase); }
            set { Set(MiddlewareOwinConstants.RequestPathBase, value); }
        }

        /// <summary>
        /// Gets or set the request path from owin.RequestPath.
        /// </summary>
        /// <returns>The request path from owin.RequestPath.</returns>
        public virtual string Path
        {
            get { return Get<string>(MiddlewareOwinConstants.RequestPath); }
            set { Set(MiddlewareOwinConstants.RequestPath, value); }
        }

        /// <summary>
        /// Gets or set the query string from owin.RequestQueryString.
        /// </summary>
        /// <returns>The query string from owin.RequestQueryString.</returns>
        public virtual string QueryString
        {
            get { return Get<string>(MiddlewareOwinConstants.RequestQueryString); }
            set { Set(MiddlewareOwinConstants.RequestQueryString, value); }
        }

        /// <summary>
        /// Gets the uniform resource identifier (URI) associated with the request.
        /// </summary>
        /// <returns>The uniform resource identifier (URI) associated with the request.</returns>
        public virtual Uri Uri => new Uri(Scheme + "://" + Host + PathBase + Path + (QueryString?.StartsWith("?") == true ? QueryString : "?" + QueryString));

        /// <summary>
        /// Gets or set the owin.RequestProtocol.
        /// </summary>
        /// <returns>The owin.RequestProtocol.</returns>
        public virtual string Protocol
        {
            get { return Get<string>(MiddlewareOwinConstants.RequestProtocol); }
            set { Set(MiddlewareOwinConstants.RequestProtocol, value); }
        }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <returns>The request headers.</returns>
        public virtual IDictionary<string, string[]> Headers => new HeaderDictionary(RawHeaders);

        private IDictionary<string, string[]> RawHeaders => Get<IDictionary<string, string[]>>(MiddlewareOwinConstants.RequestHeaders);

        /// <summary>
        /// Gets or sets the Content-Type header.
        /// </summary>
        /// <returns>The Content-Type header.</returns>
        public virtual string ContentType
        {
            get { return MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.ContentType); }
            set { MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.ContentType, value); }
        }

        /// <summary>
        /// Gets or sets the Cache-Control header.
        /// </summary>
        /// <returns>The Cache-Control header.</returns>
        public virtual string CacheControl
        {
            get { return MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.CacheControl); }
            set { MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.CacheControl, value); }
        }

        /// <summary>
        /// Gets or sets the Media-Type header.
        /// </summary>
        /// <returns>The Media-Type header.</returns>
        public virtual string MediaType
        {
            get { return MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.MediaType); }
            set { MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.MediaType, value); }
        }

        /// <summary>
        /// Gets or set the Accept header.
        /// </summary>
        /// <returns>The Accept header.</returns>
        public virtual string Accept
        {
            get { return MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.Accept); }
            set { MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.Accept, value); }
        }

        /// <summary>
        /// Gets or set the owin.RequestBody Stream.
        /// </summary>
        /// <returns>The owin.RequestBody Stream.</returns>
        public virtual Stream Body
        {
            get { return Get<Stream>(MiddlewareOwinConstants.RequestBody); }
            set { Set(MiddlewareOwinConstants.RequestBody, value); }
        }

        /// <summary>
        /// Gets or sets the cancellation token for the request.
        /// </summary>
        /// <returns>The cancellation token for the request.</returns>
        public virtual CancellationToken CallCancelled
        {
            get { return Get<CancellationToken>(MiddlewareOwinConstants.CallCancelled); }
            set { Set(MiddlewareOwinConstants.CallCancelled, value); }
        }

        /// <summary>
        /// Gets or set the server.LocalIpAddress.
        /// </summary>
        /// <returns>The server.LocalIpAddress.</returns>
        public virtual string LocalIpAddress
        {
            get { return Get<string>(MiddlewareOwinConstants.CommonKeys.LocalIpAddress); }
            set { Set(MiddlewareOwinConstants.CommonKeys.LocalIpAddress, value); }
        }

        /// <summary>
        /// Gets or set the server.LocalPort.
        /// </summary>
        /// <returns>The server.LocalPort.</returns>
        public virtual int? LocalPort
        {
            get
            {
                int value;
                if (int.TryParse(LocalPortString, out value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    LocalPortString = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    Environment.Remove(MiddlewareOwinConstants.CommonKeys.LocalPort);
                }
            }
        }

        private string LocalPortString
        {
            get { return Get<string>(MiddlewareOwinConstants.CommonKeys.LocalPort); }
            set { Set(MiddlewareOwinConstants.CommonKeys.LocalPort, value); }
        }

        /// <summary>
        /// Gets or set the server.RemoteIpAddress.
        /// </summary>
        /// <returns>The server.RemoteIpAddress.</returns>
        public virtual string RemoteIpAddress
        {
            get { return Get<string>(MiddlewareOwinConstants.CommonKeys.RemoteIpAddress); }
            set { Set(MiddlewareOwinConstants.CommonKeys.RemoteIpAddress, value); }
        }

        /// <summary>
        /// Gets or set the server.RemotePort.
        /// </summary>
        /// <returns>The server.RemotePort.</returns>
        public virtual int? RemotePort
        {
            get
            {
                int value;
                if (int.TryParse(RemotePortString, out value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    RemotePortString = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    Environment.Remove(MiddlewareOwinConstants.CommonKeys.RemotePort);
                }
            }
        }

        private string RemotePortString
        {
            get { return Get<string>(MiddlewareOwinConstants.CommonKeys.RemotePort); }
            set { Set(MiddlewareOwinConstants.CommonKeys.RemotePort, value); }
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
        public virtual IMiddlewareRequest Set<T>(string key, T value)
        {
            Environment[key] = value;
            return this;
        }
    }
}
