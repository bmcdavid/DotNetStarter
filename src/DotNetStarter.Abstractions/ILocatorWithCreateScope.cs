namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Creates/Opens a locator scope
    /// </summary>
    public interface ILocatorWithCreateScope
    {
        /// <summary>
        /// Creates/opens a locator scope
        /// </summary>
        /// <returns></returns>
        ILocatorScoped CreateScope();
    }
}