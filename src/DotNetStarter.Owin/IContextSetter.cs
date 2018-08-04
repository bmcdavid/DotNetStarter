using DotNetStarter.Owin.Abstractions;
using System.Collections.Generic;

namespace DotNetStarter.Owin
{
    /// <summary>
    /// Provides way for context to be accessed.
    /// </summary>
    public interface IContextSetter
    {
        /// <summary>
        /// Sets instances for later retrieval
        /// </summary>
        /// <param name="middleware"></param>
        /// <param name="dictionary"></param>
        void SetContexts(IMiddlewareContext middleware, IDictionary<string, object> dictionary);
    }
}