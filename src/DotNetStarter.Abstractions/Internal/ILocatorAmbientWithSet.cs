namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Allows ILocatorAmbient to set a scoped locator, internal namespace is for separation
    /// </summary>
    public interface ILocatorAmbientWithSet
    {
        /// <summary>
        /// Sets the scoped locator
        /// </summary>
        /// <param name="scopedLocator"></param>
        void SetCurrentScopedLocator(ILocatorScoped scopedLocator);
    }
}