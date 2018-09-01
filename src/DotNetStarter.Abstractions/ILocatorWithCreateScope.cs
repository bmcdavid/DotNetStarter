namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Creates/Opens a locator scope, should never be called directly, only via ILocatorScopeFactory
    /// </summary>
    public interface ILocatorWithCreateScope
    {
        /// <summary>
        /// Creates/opens a locator scope, should never be called directly, only via ILocatorScopeFactor
        /// </summary>
        /// <returns></returns>
        ILocatorScoped CreateScope();
    }
}