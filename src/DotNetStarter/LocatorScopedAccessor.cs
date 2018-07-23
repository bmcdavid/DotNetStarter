using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;

namespace DotNetStarter
{
    /// <summary>
    /// Provides access to currently scoped locator
    /// </summary>
    [Registration(typeof(ILocatorScopedAccessor), Lifecycle.Scoped)]
    public sealed class LocatorScopedAccessor : ILocatorScopedAccessor, ILocatorScopedSetter
    {
        /// <summary>
        /// Access to current scoped locator
        /// </summary>
        public ILocatorScoped CurrentScope { get; private set; }

        void ILocatorScopedSetter.SetCurrentScopedLocator(ILocatorScoped locatorScoped)
        {
            if (CurrentScope == null)
            {
                CurrentScope = locatorScoped;
            }
        }
    }
}