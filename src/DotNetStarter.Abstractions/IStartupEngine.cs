﻿using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Contains event for startup complete, ILocator for resolving, and IStartupConfiguration
    /// </summary>
    public interface IStartupEngine
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
        /// Reference to ILocator configured
        /// </summary>
        ILocator Locator { get; }
    }
}