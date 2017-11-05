using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;

namespace DotNetStarter.Internal
{
    [Register(typeof(ILocatorScopedAccessor), LifeTime.Scoped)]
    public class DefaultLocatorScopedAccessor : ILocatorScopedAccessor, ILocatorScopedSetter
    {
        private ILocatorScoped _Current;

        public ILocatorScoped CurrentScope => _Current;

        public void SetCurrentScopedLocator(ILocatorScoped locatorScoped)
        {
            _Current = locatorScoped;
        }
    }
}