// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/AspNetKatana/blob/master/LICENSE.txt for license information.
// Modifications copyright 2016 <Brad McDavid>

namespace DotNetStarter.Owin
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Context Response
    /// </summary>
    public class MiddlewareResponse : IMiddlewareResponse
    {
        /// <summary>
        /// Create a new context with only request and response header collections.
        /// </summary>
        public MiddlewareResponse()
        {
            IDictionary<string, object> environment = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                [MiddlewareOwinConstants.RequestHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase),
                [MiddlewareOwinConstants.ResponseHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            };
            Environment = environment;
        }

        /// <summary>
        /// Creates a new environment wrapper exposing response properties.
        /// </summary>
        /// <param name="environment">OWIN environment dictionary which stores state information about the request, response and relevant server state.</param>
        public MiddlewareResponse(IDictionary<string, object> environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
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
        public virtual IMiddlewareContext Context => new MiddlewareContext(Environment);

        /// <summary>
        /// Gets or sets the optional owin.ResponseStatusCode.
        /// </summary>
        /// <returns>The optional owin.ResponseStatusCode, or 200 if not set.</returns>
        public virtual int StatusCode
        {
            get { return Get<int>(MiddlewareOwinConstants.ResponseStatusCode, 200); }
            set { Set(MiddlewareOwinConstants.ResponseStatusCode, value); }
        }

        /// <summary>
        /// Gets or sets the the optional owin.ResponseReasonPhrase.
        /// </summary>
        /// <returns>The the optional owin.ResponseReasonPhrase.</returns>
        public virtual string ReasonPhrase
        {
            get { return Get<string>(MiddlewareOwinConstants.ResponseReasonPhrase); }
            set { Set(MiddlewareOwinConstants.ResponseReasonPhrase, value); }
        }

        /// <summary>
        /// Gets or sets the owin.ResponseProtocol.
        /// </summary>
        /// <returns>The owin.ResponseProtocol.</returns>
        public virtual string Protocol
        {
            get { return Get<string>(MiddlewareOwinConstants.ResponseProtocol); }
            set { Set(MiddlewareOwinConstants.ResponseProtocol, value); }
        }

        /// <summary>
        /// Gets the response header collection.
        /// </summary>
        /// <returns>The response header collection.</returns>
        public virtual IDictionary<string, string[]> Headers => new HeaderDictionary(RawHeaders);

        private IDictionary<string, string[]> RawHeaders => Get<IDictionary<string, string[]>>(MiddlewareOwinConstants.ResponseHeaders);

        /// <summary>
        /// Gets or sets the Content-Length header.
        /// </summary>
        /// <returns>The Content-Length header.</returns>
        public virtual long? ContentLength
        {
            get
            {
                if (long.TryParse(MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.ContentLength), out long value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.ContentLength,
                        value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    RawHeaders.Remove(MiddlewareOwinConstants.Headers.ContentLength);
                }
            }
        }

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
        /// Gets or sets the Expires header.
        /// </summary>
        /// <returns>The Expires header.</returns>
        public virtual DateTimeOffset? Expires
        {
            get
            {
                if (DateTimeOffset.TryParse(MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.Expires),
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.Expires,
                        value.Value.ToString(MiddlewareOwinConstants.HttpDateFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    RawHeaders.Remove(MiddlewareOwinConstants.Headers.Expires);
                }
            }
        }

        /// <summary>
        /// Gets or sets the E-Tag header.
        /// </summary>
        /// <returns>The E-Tag header.</returns>
        public virtual string ETag
        {
            get { return MiddlewareHelpers.GetHeader(RawHeaders, MiddlewareOwinConstants.Headers.ETag); }
            set { MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.ETag, value); }
        }

        /// <summary>
        /// Gets or sets the owin.ResponseBody Stream.
        /// </summary>
        /// <returns>The owin.ResponseBody Stream.</returns>
        public virtual Stream Body
        {
            get { return Get<Stream>(MiddlewareOwinConstants.ResponseBody); }
            set { Set(MiddlewareOwinConstants.ResponseBody, value); }
        }

        /// <summary>
        /// Registers for an event that fires when the response headers are sent.
        /// </summary>
        /// <param name="callback">The callback method.</param>
        /// <param name="state">The callback state.</param>
        public virtual void OnSendingHeaders(Action<object> callback, object state)
        {
            var onSendingHeaders = Get<Action<Action<object>, object>>(MiddlewareOwinConstants.CommonKeys.OnSendingHeaders);

            if (onSendingHeaders is null)
            {
                throw new NotSupportedException();
            }

            onSendingHeaders(callback, state);
        }

        /// <summary>
        /// Sets a 302 response status code and the Location header.
        /// </summary>
        /// <param name="location">The location where to redirect the client.</param>
        /// <param name="statusCode">302 status code by default, this param is for easy swap for 301s.</param>
        public virtual void Redirect(string location, int statusCode = 302)
        {
            StatusCode = statusCode;
            MiddlewareHelpers.SetHeader(RawHeaders, MiddlewareOwinConstants.Headers.Location, location);
        }

        /// <summary>
        /// Writes the given text to the response body stream using UTF-8.
        /// </summary>
        /// <param name="text">The response data.</param>
        public virtual void Write(string text)
        {
            Write(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Writes the given bytes to the response body stream.
        /// </summary>
        /// <param name="data">The response data.</param>
        public virtual void Write(byte[] data)
        {
            Write(data, 0, data is null ? 0 : data.Length);
        }

        /// <summary>
        /// Writes the given bytes to the response body stream.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="offset">The zero-based byte offset in the <paramref name="data" /> parameter at which to begin copying bytes.</param>
        /// <param name="count">The number of bytes to write.</param>
        public virtual void Write(byte[] data, int offset, int count)
        {
            Body.Write(data, offset, count);
        }

        /// <summary>
        /// Asynchronously writes the given text to the response body stream using UTF-8.
        /// </summary>
        /// <param name="text">The response data.</param>
        /// <returns>A Task tracking the state of the write operation.</returns>
        public virtual Task WriteAsync(string text) => WriteAsync(text, CancellationToken.None);

        /// <summary>
        /// Asynchronously writes the given text to the response body stream using UTF-8.
        /// </summary>
        /// <param name="text">The response data.</param>
        /// <param name="token">A token used to indicate cancellation.</param>
        /// <returns>A Task tracking the state of the write operation.</returns>
        public virtual Task WriteAsync(string text, CancellationToken token) => WriteAsync(Encoding.UTF8.GetBytes(text), token);

        /// <summary>
        /// Asynchronously writes the given bytes to the response body stream.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <returns>A Task tracking the state of the write operation.</returns>
        public virtual Task WriteAsync(byte[] data) => WriteAsync(data, CancellationToken.None);

        /// <summary>
        /// Asynchronously writes the given bytes to the response body stream.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="token">A token used to indicate cancellation.</param>
        /// <returns>A Task tracking the state of the write operation.</returns>
        public virtual Task WriteAsync(byte[] data, CancellationToken token) => WriteAsync(data, 0, data is null ? 0 : data.Length, token);

        /// <summary>
        /// Asynchronously writes the given bytes to the response body stream.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="offset">The zero-based byte offset in the <paramref name="data" /> parameter at which to begin copying bytes.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <param name="token">A token used to indicate cancellation.</param>
        /// <returns>A Task tracking the state of the write operation.</returns>
        public virtual Task WriteAsync(byte[] data, int offset, int count, CancellationToken token) => Body.WriteAsync(data, offset, count, token);

        /// <summary>
        /// Gets a value from the OWIN environment, or returns default(T) if not present.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value with the specified key or the default(T) if not present.</returns>
        public virtual T Get<T>(string key) => Get(key, default(T));

        private T Get<T>(string key, T fallback)
        {
            return Environment.TryGetValue(key, out object value) ? (T)value : fallback;
        }

        /// <summary>
        /// Sets the given key and value in the OWIN environment.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>This instance.</returns>
        public virtual IMiddlewareResponse Set<T>(string key, T value)
        {
            Environment[key] = value;
            return this;
        }
    }
}