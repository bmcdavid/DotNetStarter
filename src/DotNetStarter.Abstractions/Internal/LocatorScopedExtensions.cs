namespace DotNetStarter.Abstractions.Internal
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
        public static void SetCurrentScopedLocator(this ILocatorScoped locatorScoped)
        {
            var accessor = locatorScoped.Get<ILocatorScopedAccessor>();
            var setter = accessor as ILocatorScopedSetter;

            if (setter == null)
                throw new System.Exception($"{accessor.GetType().FullName} must implement {typeof(ILocatorScopedSetter).FullName}!");

            setter.SetCurrentScopedLocator(locatorScoped);
        }
    }
}