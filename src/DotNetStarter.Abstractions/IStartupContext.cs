﻿namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// startup Context
    /// </summary>
    [CriticalComponent]
    public interface IStartupContext
    {
        /// <summary>
        /// Service Locator
        /// </summary>
        ILocator Locator { get; }

        /// <summary>
        /// Modules to be executed
        /// </summary>
        IEnumerable<IDependencyNode> FilteredModuleTypes { get; }

        /// <summary>
        /// All discovered modules
        /// </summary>
        IEnumerable<IDependencyNode> AllModuleTypes { get; }

        /// <summary>
        /// Configuration reference
        /// </summary>
        IStartupConfiguration Configuration { get; }

        /// <summary>
        /// Gets the types that registerd locator items
        /// </summary>
        IEnumerable<Type> LocatorRegistrations { get; }
    }
}