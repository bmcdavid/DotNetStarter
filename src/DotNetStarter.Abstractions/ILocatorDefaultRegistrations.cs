namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Registers container default services before ContainerConfigure is executed
    /// </summary>
    public interface ILocatorDefaultRegistrations
    {
        /// <summary>
        /// IStartupModules added during configuration using the StartupModuleExpression
        /// </summary>
        IStartupModuleCollection StartupModuleCollection { get; set; }

        /// <summary>
        /// ILocatorConfigure modules added during configuration using the StartupModuleExpression
        /// </summary>
        ILocatorConfigureModuleCollection LocatorConfigureModuleCollection { get; set; }

        /// <summary>
        /// Registers default services and instances
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="configuration"></param>
        void Configure(ILocatorRegistry locator, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration configuration);

    }
}