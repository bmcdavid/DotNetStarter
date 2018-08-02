namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Adds debug info to locators
    /// </summary>
    public interface ILocatorWithDebugInfo
    {
        /// <summary>
        /// Debug information about container
        /// </summary>
        string DebugInfo { get; }
    }
}
