// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/AspNetKatana/blob/master/LICENSE.txt for license information.
// Modifications copyright 2016 <Brad McDavid>

namespace DotNetStarter.Owin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a wrapper for owin.RequestHeaders and owin.ResponseHeaders.
    /// </summary>
    public class HeaderDictionary : IDictionary<string, string[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Owin.HeaderDictionary" /> class.
        /// </summary>
        /// <param name="store">The underlying data store.</param>
        public HeaderDictionary(IDictionary<string, string[]> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            Store = store;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:Microsoft.Owin.HeaderDictionary" />;.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:Microsoft.Owin.HeaderDictionary" />.</returns>
        public int Count => Store.Count;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="T:Microsoft.Owin.HeaderDictionary" /> is in read-only mode.
        /// </summary>
        /// <returns>true if the <see cref="T:Microsoft.Owin.HeaderDictionary" /> is in read-only mode; otherwise, false.</returns>
        public bool IsReadOnly => Store.IsReadOnly;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection" /> that contains the keys in the <see cref="T:Microsoft.Owin.HeaderDictionary" />;.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.ICollection" /> that contains the keys in the <see cref="T:Microsoft.Owin.HeaderDictionary" />.</returns>
        public ICollection<string> Keys => Store.Keys;

        /// <summary>
        ///
        /// </summary>
        public ICollection<string[]> Values => Store.Values;

        private IDictionary<string, string[]> Store { get; }

        /// <summary>
        /// Get or sets the associated value from the collection as a single string.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection as a single string or null if the key is not present.</returns>
        public string this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        /// <summary>
        /// Throws KeyNotFoundException if the key is not present.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns></returns>
        string[] IDictionary<string, string[]>.this[string key]
        {
            get { return Store[key]; }
            set { Store[key] = value; }
        }

        /// <summary>
        /// Adds the given header and values to the collection.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header values.</param>
        public void Add(string key, string[] value)
        {
            Store.Add(key, value);
        }

        /// <summary>
        /// Adds a new list of items to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(KeyValuePair<string, string[]> item)
        {
            Store.Add(item);
        }

        /// <summary>
        /// Add a new value. Appends to the header if already present
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header value.</param>
        public void Append(string key, string value)
        {
            MiddlewareHelpers.AppendHeader(Store, key, value);
        }

        /// <summary>
        /// Quotes any values containing comas, and then coma joins all of the values with any existing values.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void AppendCommaSeparatedValues(string key, params string[] values)
        {
            MiddlewareHelpers.AppendHeaderJoined(Store, key, values);
        }

        /// <summary>
        /// Add new values. Each item remains a separate array entry.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void AppendValues(string key, params string[] values)
        {
            MiddlewareHelpers.AppendHeaderUnmodified(Store, key, values);
        }

        /// <summary>
        /// Clears the entire list of objects.
        /// </summary>
        public void Clear()
        {
            Store.Clear();
        }

        /// <summary>
        /// Returns a value indicating whether the specified object occurs within this collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>true if the specified object occurs within this collection; otherwise, false.</returns>
        public bool Contains(KeyValuePair<string, string[]> item) => Store.Contains(item);

        /// <summary>
        /// Determines whether the <see cref="T:Microsoft.Owin.HeaderDictionary" /> contains a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>true if the <see cref="T:Microsoft.Owin.HeaderDictionary" /> contains a specific key; otherwise, false.</returns>
        public bool ContainsKey(string key) => Store.ContainsKey(key);

        /// <summary>
        /// Copies the <see cref="T:Microsoft.Owin.HeaderDictionary" /> elements to a one-dimensional Array instance at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the specified objects copied from the <see cref="T:Microsoft.Owin.HeaderDictionary" />.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            Store.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the associated value from the collection as a single string.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection as a single string or null if the key is not present.</returns>
        public string Get(string key) => MiddlewareHelpers.GetHeader(Store, key);

        /// <summary>
        /// Get the associated values from the collection separated into individual values.
        /// Quoted values will not be split, and the quotes will be removed.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated values from the collection separated into individual values, or null if the key is not present.</returns>
        public IList<string> GetCommaSeparatedValues(string key)
        {
            IEnumerable<string> values = MiddlewareHelpers.GetHeaderSplit(Store, key);
            return values == null ? null : values.ToList();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator() => Store.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Get the associated values from the collection without modification.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection without modification, or null if the key is not present.</returns>
        public IList<string> GetValues(string key) => MiddlewareHelpers.GetHeaderUnmodified(Store, key);

        /// <summary>
        /// Removes the given header from the collection.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>true if the specified object was removed from the collection; otherwise, false.</returns>
        public bool Remove(string key) => Store.Remove(key);

        /// <summary>
        /// Removes the given item from the the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>true if the specified object was removed from the collection; otherwise, false.</returns>
        public bool Remove(KeyValuePair<string, string[]> item) => Store.Remove(item);

        /// <summary>
        /// Sets a specific header value.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header value.</param>
        public void Set(string key, string value)
        {
            MiddlewareHelpers.SetHeader(Store, key, value);
        }

        /// <summary>
        /// Quotes any values containing comas, and then coma joins all of the values.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void SetCommaSeparatedValues(string key, params string[] values)
        {
            MiddlewareHelpers.SetHeaderJoined(Store, key, values);
        }

        /// <summary>
        /// Sets the specified header values without modification.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void SetValues(string key, params string[] values)
        {
            MiddlewareHelpers.SetHeaderUnmodified(Store, key, values);
        }

        /// <summary>
        /// Retrieves a value from the dictionary.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the <see cref="T:Microsoft.Owin.HeaderDictionary" /> contains the key; otherwise, false.</returns>
        public bool TryGetValue(string key, out string[] value) => Store.TryGetValue(key, out value);
    }
}