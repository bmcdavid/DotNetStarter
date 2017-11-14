using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Provides access to currently scoped locator
    /// </summary>
    [Registration(typeof(ILocatorScopedAccessor), Lifecycle.Scoped)]
    public sealed class LocatorScopedAccessor : ILocatorScopedAccessor, ILocatorScopedSetter
    {
        private ILocatorScoped _Current;

        /// <summary>
        /// Access to current scoped locator
        /// </summary>
        public ILocatorScoped CurrentScope => _Current;

        void ILocatorScopedSetter.SetCurrentScopedLocator(ILocatorScoped locatorScoped)
        {
            if (_Current == null)
            {
                _Current = locatorScoped;
            }
        }
    }
}