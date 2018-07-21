namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides an ambient Locator where Current may be the current scoped open, or the singleton locator within the startup context
    /// </summary>
    public interface ILocatorAmbient
    {
        /// <summary>
        /// Scoped or StartupContext's locator
        /// </summary>
        ILocator Current { get; }

        /// <summary>
        /// Quick check to determine if current is in a scope
        /// </summary>
        bool IsScoped { get; }
    }
}