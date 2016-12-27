namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Used to disable modules
    /// </summary>
    public interface IStartupModuleFilter
    {
        /// <summary>
        /// Allows for filtering of discovered startup modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        IEnumerable<IDependencyNode> FilterModules(IEnumerable<IDependencyNode> modules);
    }
}
