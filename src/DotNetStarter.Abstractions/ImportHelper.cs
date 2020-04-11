using System;
using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides access to set an ILocator for Import&lt;T> calls
    /// </summary>
    public static class ImportHelper
    {
        /// <summary>
        /// Event to set a locator
        /// </summary>
        public static event Func<ILocator> OnEnsureLocator;

        /// <summary>
        /// Test helper for mocking dependencies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public static Import<T> New<T>(T instance = null, ICollection<T> all = null)
            where T : class
        {
            return new Import<T>
            {
                Accessor = new ImportAccessor<T>(instance, all ?? new List<T>())
            };
        }

        /// <summary>
        /// Test helper for mocking dependencies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ImportFactory<T> New<T>(Func<T> factory)
            where T : class
        {
            return new ImportFactory<T>
            {
                Accessor = new ImportFactoryAccessor<T>(factory)
            };
        }

        private static ILocator _Locator;

        internal static ILocator Locator => EnsureLocator();

        private static ILocator EnsureLocator()
        {
            if (_Locator is object) { return _Locator; }

            _Locator = OnEnsureLocator?.Invoke();

            if (_Locator is null)
            {
                throw new NullReferenceException($"A {typeof(ILocator)} was not set for Import<T>, please attach event to {typeof(ImportHelper).FullName}.{nameof(OnEnsureLocator)}!");
            }

            return _Locator;
        }
    }
}