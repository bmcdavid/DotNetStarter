using DotNetStarter.Abstractions;
using System.Collections.Generic;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Access to filtered and sorted modules
    /// </summary>
    public sealed class StartupTaskModuleCollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sortedModules"></param>
        /// <param name="filteredModules"></param>
        public StartupTaskModuleCollection(ICollection<IDependencyNode> sortedModules, ICollection<IDependencyNode> filteredModules)
        {
            SortedModules = sortedModules;
            FilteredModules = filteredModules;
        }

        /// <summary>
        /// Sorted modules
        /// </summary>
        public ICollection<IDependencyNode> SortedModules { get; }

        /// <summary>
        /// Filtered modules
        /// </summary>
        public ICollection<IDependencyNode> FilteredModules { get; }
    }
}