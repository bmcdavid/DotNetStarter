using DotNetStarter.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetStarter
{
    /// <summary>
    /// Provides mechanism to pass items between modules
    /// </summary>
    public class StartupEnvironmentItemCollection : IItemCollection
    {
        private IDictionary<Type, object> items = new Dictionary<Type, object>();

        /// <summary>
        /// Constructor
        /// </summary>
        public StartupEnvironmentItemCollection() { }

        /// <summary>
        /// Get or set items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[Type key]
        {
            get
            {
                if (key == null) { throw new ArgumentNullException(nameof(key)); }
                if (items.TryGetValue(key, out var obj)) { return obj; }

                return null;
            }
            set
            {
                if (key == null) { throw new ArgumentNullException(nameof(key)); }
                if (value == null) { items.Remove(key); }

                items[key] = value;
            }
        }

        /// <summary>
        /// Is Read only
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}