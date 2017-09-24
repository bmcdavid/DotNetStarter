using System;

namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Must be implemented on IStartupConfiguration implementations to delay Startup task
    /// </summary>
    public interface IStartupDelayed
    {
        /// <summary>
        /// When true, the IStartupHandler will not execute startup immediately, it will wait until InvokeDelayedStartup()
        /// </summary>
        bool EnableDelayedStartup { get; }

        /// <summary>
        /// Action to perform delayed startup
        /// </summary>
        Action DelayedStartup { get; set; }
    }

}