using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Provides access to currently scoped locator
    /// </summary>
    [Register(typeof(ILocatorScopedAccessor), LifeTime.Scoped)]
    public class DefaultLocatorScopedAccessor : ILocatorScopedAccessor, ILocatorScopedSetter
    {
        private ILocatorScoped _Current;

        /// <summary>
        /// Access to current scoped locator
        /// </summary>
        public ILocatorScoped CurrentScope => _Current;

        /// <summary>
        /// Sets the current scope, done via ILocatorScope implementation constructor
        /// </summary>
        /// <param name="locatorScoped"></param>
        public void SetCurrentScopedLocator(ILocatorScoped locatorScoped)
        {
            if (_Current == null)
            {
                _Current = locatorScoped;
            }
        }
    }
}