using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Default ILocatorScopeFactory implementation
    /// </summary>
    [Register(typeof(ILocatorScopeFactory), LifeTime.Singleton)]
    public class LocatorScopeFactory : ILocatorScopeFactory
    {
        private readonly ILocator _Locator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unscopedLocator"></param>
        public LocatorScopeFactory(ILocator unscopedLocator)
        {
            _Locator = unscopedLocator;
        }

        /// <summary>
        /// Creates a child scope of given scoped locator
        /// </summary>
        /// <param name="locatorScoped"></param>
        /// <returns></returns>
        public ILocatorScoped CreateChildScope(ILocatorScoped locatorScoped)
        {
            return _Create(locatorScoped);
        }

        /// <summary>
        /// Creates an initial scope
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope()
        {
            return _Create(_Locator);
        }

        private ILocatorScoped _Create(ILocator locator)
        {
            var creator = locator as ILocatorCreateScope;

            if (creator == null)
                throw new System.ArgumentException($"{locator.GetType().FullName} doesn't implement {typeof(ILocatorCreateScope).FullName}!");

            return creator.CreateScope();
        }
    }
}