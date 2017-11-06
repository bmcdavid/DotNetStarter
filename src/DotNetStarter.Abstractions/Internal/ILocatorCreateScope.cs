namespace DotNetStarter.Abstractions.Internal
{
    //todo: remove ILocatorCreateScope,ILocatorScoped,ILocatorScopedAccessor, ILocatorScopeFactory, ILocatorScopedSetter from internal

    /// <summary>
    /// Creates/Opens a locator scope
    /// </summary>
    public interface ILocatorCreateScope
    {
        /// <summary>
        /// Creates/opens a locator scope
        /// </summary>
        /// <returns></returns>
        ILocatorScoped CreateScope();
    }
}
