using DotNetStarter.Owin.Abstractions;
using System.Collections.Generic;

namespace DotNetStarter.Owin
{
    /// <summary>
    /// Provides access to Owin middleware context and dictionary
    /// </summary>
    public interface IContextAccessor
    {
        /// <summary>
        /// Current context dictionary
        /// </summary>
        IDictionary<string, object> CurrentDictionaryContext { get; }

        /// <summary>
        /// Current context middleware
        /// </summary>
        IMiddlewareContext CurrentMiddlewareContext { get; }
    }
}
