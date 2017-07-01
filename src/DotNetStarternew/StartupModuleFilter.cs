namespace DotNetStarter
{
    using Abstractions;
    using System.Collections.Generic;

    /// <summary>
    /// Default module filter, excludes nothing
    /// </summary>
    public class StartupModuleFilter : IStartupModuleFilter
    {
        /// <summary>
        /// Default implementation filters nothing
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public virtual IEnumerable<IDependencyNode> FilterModules(IEnumerable<IDependencyNode> modules) => modules;
    }
}
