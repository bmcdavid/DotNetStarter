using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Represents an HTTP module request scope kind
    /// </summary>
    public class HttpRequestScopeKind : IScopeKind
    {
        private HttpRequestScopeKind() { }

        /// <summary>
        /// Singleton accessor
        /// </summary>
        public static readonly IScopeKind HttpRequest = new HttpRequestScopeKind();

        static Type _Type = typeof(HttpRequestScopeKind);

        /// <summary>
        /// Scope Type
        /// </summary>
        public Type ScopeType => _Type;

        /// <summary>
        /// Scope name
        /// </summary>
        public string Name => "HttpRequest";
    }
}
