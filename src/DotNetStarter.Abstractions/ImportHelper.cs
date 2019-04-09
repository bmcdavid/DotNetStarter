using System;

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