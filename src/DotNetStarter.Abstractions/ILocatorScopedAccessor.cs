namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides access to current scoped locator, and should ONLY be injected in Scoped or Transient services
    /// </summary>
    public interface ILocatorScopedAccessor
    {
        /// <summary>
        /// Current scoped locator
        /// </summary>
        ILocatorScoped CurrentScope { get; }
    }
}