using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides DRY for setting current scoped locator;
    /// </summary>
    public static class LocatorScopedExtensions
    {
        /// <summary>
        /// Sets current scoped locator or throws exception if ILocatorScopedAccessor does not implement ILocatorScopedSetter
        /// </summary>
        /// <param name="locatorScoped"></param>
        [Obsolete]
        public static void SetCurrentScopedLocator(this ILocatorScoped locatorScoped)
        {
            var accessor = locatorScoped.Get<ILocatorScopedAccessor>();

            if (!(accessor is ILocatorScopedSetter setter))
            {
                throw new Exception($"{accessor.GetType().FullName} must implement {typeof(ILocatorScopedSetter).FullName}!");
            }

            setter.SetCurrentScopedLocator(locatorScoped);
        }
    }
}