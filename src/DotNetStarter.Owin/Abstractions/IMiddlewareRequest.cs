﻿namespace DotNetStarter.Owin.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// Owin Request Object
    /// </summary>
    public interface IMiddlewareRequest
    {
        /// <summary>
        /// Gets the OWIN environment.
        /// </summary>
        /// <returns>The OWIN environment.</returns>
        IDictionary<string, object> Environment { get; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <returns>The request context.</returns>
        IMiddlewareContext Context { get; }

        /// <summary>
        /// Gets or set the HTTP method.
        /// </summary>
        /// <returns>The HTTP method.</returns>
        string Method { get; set; }

        /// <summary>
        /// Gets or set the HTTP request scheme from owin.RequestScheme.
        /// </summary>
        /// <returns>The HTTP request scheme from owin.RequestScheme.</returns>
        string Scheme { get; set; }

        /// <summary>
        /// Returns true if the owin.RequestScheme is https.
        /// </summary>
        /// <returns>true if this request is using https; otherwise, false.</returns>
        bool IsSecure { get; }

        /// <summary>
        /// Gets or set the Host header. May include the port.
        /// </summary>
        /// <return>The Host header.</return>
        string Host { get; set; }

        /// <summary>
        /// Gets or set the owin.RequestPathBase.
        /// </summary>
        /// <returns>The owin.RequestPathBase.</returns>
        string PathBase { get; set; }

        /// <summary>
        /// Gets or set the request path from owin.RequestPath.
        /// </summary>
        /// <returns>The request path from owin.RequestPath.</returns>
        string Path { get; set; }

        /// <summary>
        /// Gets or set the query string from owin.RequestQueryString.
        /// </summary>
        /// <returns>The query string from owin.RequestQueryString.</returns>
        string QueryString { get; set; }

        /// <summary>
        /// Gets the uniform resource identifier (URI) associated with the request.
        /// </summary>
        /// <returns>The uniform resource identifier (URI) associated with the request.</returns>
        Uri Uri { get; }

        /// <summary>
        /// Gets or set the owin.RequestProtocol.
        /// </summary>
        /// <returns>The owin.RequestProtocol.</returns>
        string Protocol { get; set; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <returns>The request headers.</returns>
        IDictionary<string, string[]> Headers { get; }
        
        /// <summary>
        /// Gets or sets the Content-Type header.
        /// </summary>
        /// <returns>The Content-Type header.</returns>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the Cache-Control header.
        /// </summary>
        /// <returns>The Cache-Control header.</returns>
        string CacheControl { get; set; }

        /// <summary>
        /// Gets or sets the Media-Type header.
        /// </summary>
        /// <returns>The Media-Type header.</returns>
        string MediaType { get; set; }

        /// <summary>
        /// Gets or set the Accept header.
        /// </summary>
        /// <returns>The Accept header.</returns>
        string Accept { get; set; }

        /// <summary>
        /// Gets or set the owin.RequestBody Stream.
        /// </summary>
        /// <returns>The owin.RequestBody Stream.</returns>
        Stream Body { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token for the request.
        /// </summary>
        /// <returns>The cancellation token for the request.</returns>        
        CancellationToken CallCancelled { get; set; }

        /// <summary>
        /// Gets or set the server.LocalIpAddress.
        /// </summary>
        /// <returns>The server.LocalIpAddress.</returns>
        string LocalIpAddress { get; set; }

        /// <summary>
        /// Gets or set the server.LocalPort.
        /// </summary>
        /// <returns>The server.LocalPort.</returns>
        int? LocalPort { get; set; }

        /// <summary>
        /// Gets or set the server.RemoteIpAddress.
        /// </summary>
        /// <returns>The server.RemoteIpAddress.</returns>
        string RemoteIpAddress { get; set; }

        /// <summary>
        /// Gets or set the server.RemotePort.
        /// </summary>
        /// <returns>The server.RemotePort.</returns>
        int? RemotePort { get; set; }

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
        IMiddlewareRequest Set<T>(string key, T value);
    }
}
