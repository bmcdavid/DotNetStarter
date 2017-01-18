namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Registers container default services before ContainerConfigure is executed
    /// </summary>
    public interface ILocatorDefaultRegistrations
    {
        /// <summary>
        /// Registers default services and instances
        /// </summary>
        /// <param name="container"></param>
        /// <param name="filteredModules"></param>
        /// <param name="configuration"></param>
        /// <param name="objectFactory"></param>
        void Configure(ILocatorRegistry container, IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration configuration, IStartupObjectFactory objectFactory);
    }
}