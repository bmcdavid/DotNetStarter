using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Allows locator to select how ILocatorConfigure objects are creates
    /// </summary>
    public interface ILocatorResolveConfigureModules
    {
        /// <summary>
        /// Allows locator to select how ILocatorConfigure objects are creates
        /// </summary>
        /// <param name="filteredModules"></param>
        /// <param name="startupConfiguration"></param>
        /// <returns></returns>
        IEnumerable<ILocatorConfigure> ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules, IStartupConfiguration startupConfiguration);
    }

    /// <summary>
    /// Allows ILocatorRegistry to perform any final tasks before container setup complete is invoked
    /// </summary>
    public interface ILocatorVerification
    {
        /// <summary>
        /// Allows ILocatorRegistry to perform any final tasks before container setup complete is invoked
        /// </summary>
        void Verify();
    }
}
