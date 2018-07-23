﻿namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Registers container default services before ContainerConfigure is executed
    /// </summary>
    public interface ILocatorDefaultRegistrations
    {
#pragma warning disable CS0612 // Type or member is obsolete
        /// <summary>
        /// Registers default services and instances
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="configuration"></param>
        /// <param name="objectFactory"></param>
        void Configure(ILocatorRegistry locator, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration configuration, IStartupObjectFactory objectFactory);
#pragma warning restore CS0612 // Type or member is obsolete
    }

    //todo: IlocatorDefaultRegistrationsWithCollections is a temp fix until next breaking change, these should be moved to ILocatorDefaultRegistrations
    /// <summary>
    /// Provides access to StartupBuilder registered modules
    /// </summary>
    public interface ILocatorDefaultRegistrationsWithCollections : ILocatorDefaultRegistrations
    {
        /// <summary>
        /// IStartupModules added during configuration using the StartupModuleExpression
        /// </summary>
        IStartupModuleCollection StartupModuleCollection { get; set; }

        /// <summary>
        /// ILocatorConfigure modules added during configuration using the StartupModuleExpression
        /// </summary>
        ILocatorConfigureModuleCollection LocatorConfigureModuleCollection { get; set; }
    }
}