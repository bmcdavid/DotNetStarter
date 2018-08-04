namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Sorts a set of nodes by dependency count, then alpha.
    /// </summary>
    public interface IDependencySorter
    {
        /// <summary>
        /// Sorts given types by T dependencies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        /// <returns>IEnumerable&lt;DependencyNode> or InvalidOperationException</returns>
        IList<IDependencyNode> Sort<T>(IEnumerable<object> nodes) where T : StartupDependencyBaseAttribute;
    }
}