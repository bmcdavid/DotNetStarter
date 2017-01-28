namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to set an ILocator for Import&lt;T> calls
    /// </summary>
    public static class ImportHelper
    {
        /// <summary>
        /// Event to set a locator
        /// </summary>
        public static event Func<ILocator> OnEnsureLocator;

        static ILocator _Locator;

        internal static ILocator Locator => EnsureLocator();

        static ILocator EnsureLocator()
        {
            if (_Locator != null) return _Locator;

            _Locator = OnEnsureLocator?.Invoke();

            if (_Locator == null)
                throw new NullReferenceException($"A {typeof(ILocator)} was not set for Import<T>, please attach event to {nameof(OnEnsureLocator)}!");

            return _Locator;
        }
    }

    /// <summary>
    /// Provides access to locator services
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public struct Import<TService> where TService : class
    {
        /// <summary>
        /// Access to a single service
        /// </summary>
        public TService Service
        {
            get
            {
                return ImportHelper.Locator.Get<TService>();
            }
        }

        /// <summary>
        /// Access to all services registered
        /// </summary>
        public IEnumerable<TService> AllServices
        {
            get
            {
                return ImportHelper.Locator.GetAll<TService>();
            }
        }
    }
}