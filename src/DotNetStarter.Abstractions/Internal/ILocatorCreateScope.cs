namespace DotNetStarter.Abstractions.Internal
{
    //todo: v2 remove ILocatorCreateScope,ILocatorScoped, and IScopeKind from internal

    /// <summary>
    /// Creates/Opens a locator scope
    /// </summary>
    public interface ILocatorCreateScope
    {
        /// <summary>
        /// Creates/opens a locator scope
        /// </summary>
        /// <param name="scopeKind"></param>
        /// <returns></returns>
        ILocatorScoped CreateScope(IScopeKind scopeKind);
    }
}
