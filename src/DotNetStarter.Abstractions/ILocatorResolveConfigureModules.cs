using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Allows locator implementation to select how ILocatorConfigure objects are created
    /// </summary>
    public interface ILocatorRegistryWithResolveConfigureModules
    {
        /// <summary>
        /// Allows locator implementation to select how ILocatorConfigure objects are created
        /// </summary>
        /// <param name="filteredModules"></param>
        /// <param name="startupConfiguration"></param>
        /// <returns></returns>
        IEnumerable<ILocatorConfigure> ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration startupConfiguration);
    }
}