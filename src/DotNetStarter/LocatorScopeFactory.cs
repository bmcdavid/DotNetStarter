using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Default ILocatorScopeFactory implementation
    /// </summary>
    [Registration(typeof(ILocatorScopedFactory), Lifecycle.Singleton)]
    public class LocatorScopeFactory : ILocatorScopedFactory
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
        public virtual ILocatorScoped CreateChildScope(ILocatorScoped locatorScoped)
        {
            return _Create(locatorScoped);
        }

        /// <summary>
        /// Creates an initial scope
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope()
        {
            return _Create(_Locator);
        }

        private ILocatorScoped _Create(ILocator locator)
        {
            var creator = locator as ILocatorCreateScope;

            if (creator == null)
                throw new System.ArgumentException($"{locator.GetType().FullName} doesn't implement {typeof(ILocatorCreateScope).FullName}!");

            var scope = creator.CreateScope();
            scope.SetCurrentScopedLocator();

            return scope;
        }
    }
}