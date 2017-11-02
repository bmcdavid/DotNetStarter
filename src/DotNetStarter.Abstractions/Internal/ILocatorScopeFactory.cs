namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Creates scoped locators
    /// </summary>
    public interface ILocatorScopeFactory
    {
        /// <summary>
        /// Creates a child scope of given scoped locator
        /// </summary>
        /// <param name="locatorScoped"></param>
        /// <returns></returns>
        ILocatorScoped CreateChildScope(ILocatorScoped locatorScoped);

        /// <summary>
        /// Creates an initial scope
        /// </summary>
        /// <param name="scopeKind"></param>
        /// <returns></returns>
        ILocatorScoped CreateScope(IScopeKind scopeKind);
    }
}