using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Default ILocatorAmbient
    /// </summary>
    [Registration(typeof(ILocatorAmbient), Lifecycle.Singleton)]
    public sealed class LocatorAmbient : ILocatorAmbient, ILocatorAmbientWithSet
    {
        private readonly ILocator _unscopedLocator;

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
                var stack = GetStack();
                return stack?.Count > 0 ? stack.Peek() : _unscopedLocator;
            }
        }

        void ILocatorAmbientWithSet.SetCurrentScopedLocator(ILocatorScoped scopedLocator)
        {
            AddLocator(scopedLocator);
        }

        private static void AddLocator(ILocatorScoped scopedLocator)
        {
            var stack = GetStack() ?? new Stack<ILocatorScoped>();
            // clears on scope disposing
            if (scopedLocator == null && stack.Count > 0) { stack.Pop(); }
            // push newest scope to top
            else if (scopedLocator != null) { stack.Push(scopedLocator); }
            else
            {
                throw new Exception("Null locator tried to add to remove from empty stack, or null tried to add to stack!");
            }
            SetStack(stack);
        }

#if NET45 || NET40 || NET35
        //based on httpcontext
        private static readonly string Key = typeof(LocatorAmbient).FullName;

        private static Stack<ILocatorScoped> GetStack()
        {
            return System.Runtime.Remoting.Messaging.CallContext.GetData(Key) as Stack<ILocatorScoped>;
        }

        private static void SetStack(Stack<ILocatorScoped> stack)
        {
            System.Runtime.Remoting.Messaging.CallContext.SetData(Key, stack);
        }
#elif NETSTANDARD2_0 || NETSTANDARD1_3 || NETSTANDARD1_6
        // based on httpcontextaccessor
        private static readonly System.Threading.AsyncLocal<Stack<ILocatorScoped>> LocatorScopedContext = new System.Threading.AsyncLocal<Stack<ILocatorScoped>>();

        private static Stack<ILocatorScoped> GetStack()
        {
            return LocatorScopedContext.Value;
        }

        private static void SetStack(Stack<ILocatorScoped> stack)
        {
            LocatorScopedContext.Value = stack;
        }
#else
        private static Stack<ILocatorScoped> GetStack()
        {
            throw new NotSupportedException("ILocatorAmbient not supported in netstandard1.0 and netstandard1.1!");
        }

        private static void SetStack(Stack<ILocatorScoped> stack)
        {
            throw new NotSupportedException("ILocatorAmbient not supported in netstandard1.0 and netstandard1.1!");
        }
#endif
    }
}