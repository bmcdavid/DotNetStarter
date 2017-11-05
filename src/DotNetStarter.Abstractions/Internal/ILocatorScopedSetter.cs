namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Sets current scope of ILocatorScopedAccessor, must be implemented on same type as ILocatorScopedAccessor
    /// </summary>
    public interface ILocatorScopedSetter
    {
        /// <summary>
        /// Sets current scoped locator
        /// </summary>
        /// <param name="locatorScoped"></param>
        void SetCurrentScopedLocator(ILocatorScoped locatorScoped);
    }
}