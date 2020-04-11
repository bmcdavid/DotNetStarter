using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Default ILocatorAmbient
    /// </summary>
    [Registration(typeof(ILocatorAmbient), Lifecycle.Singleton)]
    public sealed class LocatorAmbient : ILocatorAmbient, ILocatorAmbientWithSet
    {
        private static readonly string _key = typeof(LocatorAmbient).FullName;
        private readonly ILocator _unscopedLocator;
        private Func<IDictionary> _contextStorage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unscopedLocator"></param>
        public LocatorAmbient(ILocator unscopedLocator) => _unscopedLocator = unscopedLocator;

        /// <summary>
        /// Determines if in a scope
        /// </summary>
        public bool IsScoped => Current is ILocatorScoped;

        /// <summary>
        /// Current ILocator which may be scoped
        /// </summary>
        public ILocator Current
        {
            get
            {
                var stack = GetStack(_contextStorage);
                return stack?.Count > 0 ? stack.Peek() : _unscopedLocator;
            }
        }

        void ILocatorAmbientWithSet.SetCurrentScopedLocator(ILocatorScoped scopedLocator) =>
            AddLocator(scopedLocator, _contextStorage);

        /// <summary>
        /// Assigns preferred storage, ie HttpContext.Current.Items for ASP.Net Framework
        /// </summary>
        /// <param name="contextStorage"></param>
        public void PreferredStorageAccessor(Func<IDictionary> contextStorage) => _contextStorage = contextStorage;

        private static void AddLocator(ILocatorScoped scopedLocator, Func<IDictionary> preferredStorage)
        {
            var stack = GetStack(preferredStorage) ?? new Stack<ILocatorScoped>();
            // clears on scope disposing
            if (scopedLocator is null && stack.Count > 0) { stack.Pop(); }
            // push newest scope to top
            else if (scopedLocator is object) { stack.Push(scopedLocator); }
            SetStack(stack, preferredStorage);
        }

#if HAS_ASYNC_LOCAL
        // based on httpcontextaccessor
        private static readonly System.Threading.AsyncLocal<Stack<ILocatorScoped>> LocatorScopedContext = new System.Threading.AsyncLocal<Stack<ILocatorScoped>>();

        private static Stack<ILocatorScoped> GetStack(Func<IDictionary> preferredStorage)
        {
            var storageDictionary = preferredStorage?.Invoke();
            if (storageDictionary is null)
            {
                return LocatorScopedContext.Value;
            }

            return storageDictionary[_key] as Stack<ILocatorScoped>;
        }

        private static void SetStack(Stack<ILocatorScoped> stack, Func<IDictionary> preferredStorage)
        {
            if (preferredStorage?.Invoke() is IDictionary storage)
            {
                storage[_key] = stack;

                return;
            }

            LocatorScopedContext.Value = stack;

        }
#elif NETFULLFRAMEWORK

        //based on httpcontext
        private static Stack<ILocatorScoped> GetStack(Func<IDictionary> preferredStorage)
        {
            var storageDictionary = preferredStorage?.Invoke();
            if (storageDictionary is null)
            {
                return System.Runtime.Remoting.Messaging.CallContext.GetData(_key) as Stack<ILocatorScoped>;
            }

            return storageDictionary[_key] as Stack<ILocatorScoped>;
        }

        private static void SetStack(Stack<ILocatorScoped> stack, Func<IDictionary> preferredStorage)
        {
            if (preferredStorage?.Invoke() is IDictionary storage)
            {
                storage[_key] = stack;

                return;
            }

            System.Runtime.Remoting.Messaging.CallContext.SetData(_key, stack);
        }
#else
        private static Stack<ILocatorScoped> GetStack(Func<IDictionary> preferredStorage) { return null; }

        private static void SetStack(Stack<ILocatorScoped> stack, Func<IDictionary> preferredStorage) { }
#endif
    }
}