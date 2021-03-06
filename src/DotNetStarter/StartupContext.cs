﻿namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Default startup context
    /// </summary>
    public class StartupContext : IStartupContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="allModules"></param>
        /// <param name="filteredModules"></param>
        /// <param name="startupConfiguration"></param>
        public StartupContext(ILocator locator, IEnumerable<IDependencyNode> allModules, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration startupConfiguration)
        {
            Locator = locator;
            AllModuleTypes = allModules;
            FilteredModuleTypes = filteredModules;
            Configuration = startupConfiguration;
        }

        /// <summary>
        /// List of discovered modules
        /// </summary>
        public IEnumerable<IDependencyNode> AllModuleTypes { get; }

        /// <summary>
        /// List of executing modules
        /// </summary>
        public IEnumerable<IDependencyNode> FilteredModuleTypes { get; }

        /// <summary>
        /// Configuration reference
        /// </summary>
        public IStartupConfiguration Configuration { get; }

        /// <summary>
        /// Service locator
        /// </summary>
        public ILocator Locator { get; }

        /// <summary>
        /// Gets the types that registerd locator items
        /// </summary>
        public IEnumerable<Type> LocatorRegistrations
        {
            get
            {
                return FilteredModuleTypes?.Where(x => typeof(ILocatorConfigure)
                            .IsAssignableFromCheck(x.Node as Type))?.Select(x => x.Node as Type);
            }
        }
    }
}