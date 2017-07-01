namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// A dependency node for sorting
    /// </summary>
    public interface IDependencyNode
    {
        /// <summary>
        /// List of dependencies
        /// </summary>
        HashSet<object> Dependencies { get; }

        /// <summary>
        /// Count of dependencies
        /// </summary>
        int DependencyCount { get; }

        /// <summary>
        /// Node's full name
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Determines if node is type or assembly
        /// </summary>
        bool IsAssembly { get; }

        /// <summary>
        /// Node instance
        /// </summary>
        object Node { get; }

        /// <summary>
        /// Node attribute
        /// </summary>
        DependencyBaseAttribute NodeAttribute { get; }
    }
}