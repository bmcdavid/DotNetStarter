// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/HttpAbstractions/blob/master/LICENSE.txt for license information.
// Modifications copyright 2018 <Brad McDavid>

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
        /// Get or set items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[Type key]
        {
            get
            {
                if (key is null) { throw new ArgumentNullException(nameof(key)); }
                if (items.TryGetValue(key, out var obj)) { return obj; }

                return null;
            }
            set
            {
                if (key is null) { throw new ArgumentNullException(nameof(key)); }
                if (value is null) { items.Remove(key); }

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