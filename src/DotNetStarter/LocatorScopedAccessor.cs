using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Provides access to currently scoped locator
    /// </summary>
    [Register(typeof(ILocatorScopedAccessor), LifeTime.Scoped)]
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