using DotNetStarter.Abstractions;
using DotNetStarter.Owin.Abstractions;
using System.Collections.Generic;

namespace DotNetStarter.Owin
{
    /// <summary>
    /// Default context accessor
    /// </summary>
    [Registration(typeof(IContextAccessor), Lifecycle.Scoped)]
    public sealed class ContextAccessor : IContextAccessor, IContextSetter
    {
        /// <summary>
        /// Current dictionary
        /// </summary>
        public IDictionary<string, object> CurrentDictionaryContext { get; private set; }

        /// <summary>
        /// Current middleware context
        /// </summary>
        public IMiddlewareContext CurrentMiddlewareContext { get; private set; }

        void IContextSetter.SetContexts(IMiddlewareContext middleware, IDictionary<string, object> dictionary)
        {
            if (CurrentMiddlewareContext == null)
            {
                CurrentMiddlewareContext = middleware;
            }

            if (CurrentDictionaryContext == null)
            {
                CurrentDictionaryContext = dictionary;
            }
        }
    }
}
