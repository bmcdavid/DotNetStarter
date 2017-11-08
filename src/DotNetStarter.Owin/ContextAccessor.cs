using DotNetStarter.Abstractions;
using DotNetStarter.Owin.Abstractions;
using System.Collections.Generic;

namespace DotNetStarter.Owin
{
    /// <summary>
    /// Default context accessor
    /// </summary>
    [Register(typeof(IContextAccessor), LifeTime.Scoped)]
    public sealed class ContextAccessor : IContextAccessor, IContextSetter
    {
        private IDictionary<string, object> _Dictionary;
        private IMiddlewareContext _Middleware;

        /// <summary>
        /// Current dictionary
        /// </summary>
        public IDictionary<string, object> CurrentDictionaryContext => _Dictionary;

        /// <summary>
        /// Current middleware context
        /// </summary>
        public IMiddlewareContext CurrentMiddlewareContext => _Middleware;

        void IContextSetter.SetContexts(IMiddlewareContext middleware, IDictionary<string, object> dictionary)
        {
            if (_Middleware == null)
            {
                _Middleware = middleware;
            }

            if (_Dictionary == null)
            {
                _Dictionary = dictionary;
            }
        }
    }
}
