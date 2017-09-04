using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Contains events for intialization
    /// </summary>
    public interface IStartupEngine : IDisposable
    {
        /// <summary>
        /// Executes when locator startup is complete
        /// </summary>
        event Action OnLocatorStartupComplete;

        /// <summary>
        /// Executes when startup has completed tasks
        /// </summary>
        event Action OnStartupComplete;

        /// <summary>
        /// Configuration reference
        /// </summary>
        IStartupConfiguration Configuration { get; }

        /// <summary>
        /// Reference to locator
        /// </summary>
        ILocator Locator { get; }
    }
}