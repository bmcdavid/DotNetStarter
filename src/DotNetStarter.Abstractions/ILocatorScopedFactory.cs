namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Creates scoped locators
    /// </summary>
    public interface ILocatorScopedFactory
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
        /// <returns></returns>
        ILocatorScoped CreateScope();
    }
}