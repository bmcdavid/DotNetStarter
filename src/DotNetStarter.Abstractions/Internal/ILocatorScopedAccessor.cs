namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Provides access to current scoped locator, and should only be injected in Scoped or Transient services
    /// </summary>
    public interface ILocatorScopedAccessor
    {
        /// <summary>
        /// Current scoped locator
        /// </summary>
        ILocatorScoped CurrentScope { get; }
    }
}