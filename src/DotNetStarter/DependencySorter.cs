using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter
{
    /// <summary>
    /// Default dependency sorter
    /// </summary>
    public class DependencySorter : IDependencySorter
    {
        private readonly Func<object, Type, IDependencyNode> _DependencyNodeFactory;
        private readonly IComparer<IDependencyNode> _depdendencyComparer;

        /// <summary>
        /// DI constructor
        /// </summary>
        /// <param name="dependencyNodeFactory"></param>
        /// <param name="dependencyComparer"></param>
        public DependencySorter(Func<object, Type, IDependencyNode> dependencyNodeFactory, IComparer<IDependencyNode> dependencyComparer = null)
        {
            _DependencyNodeFactory = dependencyNodeFactory ?? throw new ArgumentNullException(nameof(dependencyNodeFactory));
            _depdendencyComparer = dependencyComparer ?? new DependencyComparer();
        }

        /// <summary>
        /// Default sorter for Types or Assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public virtual IList<IDependencyNode> Sort<T>(IEnumerable<object> nodes) where T : StartupDependencyBaseAttribute
        {
            var properSort = new List<IDependencyNode>();
            var initalSort = new Queue<IDependencyNode>(nodes.Select(n => _DependencyNodeFactory(n, typeof(T))).OrderBy(_ => _, _depdendencyComparer));
            var dependencyChecks = new HashSet<object>();
            int index = 0, unresolvedCount = initalSort.Count;

            while (initalSort.Count > 0)
            {
                index++;
                var node = initalSort.Dequeue();
                if (node.Dependencies.IsSubsetOf(dependencyChecks))
                {
                    properSort.Add(node);
                    dependencyChecks.Add(node.Node);
                    continue;
                }

                initalSort.Enqueue(node);// requeue for later pass

                if (index == unresolvedCount)
                {
                    if (initalSort.Count == unresolvedCount) { break; }
                    index = 0;
                    unresolvedCount = initalSort.Count;
                }
            }

            if (initalSort.Count > 0)
            {
                var names = string.Join(Environment.NewLine, initalSort.Select(x => x.FullName).ToArray());
                throw new InvalidOperationException($"Cannot resolve ({names}) for attribute {typeof(T).FullName}, please check their dependencies!");
            }

            return properSort;
        }
    }
}