using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Contains events for ILocatorConfigure tasks and IStartupConfiguration
    /// </summary>
    [CriticalComponent]
    public interface ILocatorConfigureEngine
    {
        /// <summary>
        /// Executes when startup has completed tasks
        /// </summary>
        event Action OnStartupComplete;

        /// <summary>
        /// Configuration reference
        /// </summary>
        IStartupConfiguration Configuration { get; }

        /// <summary>
        /// Executes when locator startup is complete
        /// </summary>
        event Action OnLocatorStartupComplete;
    }
}