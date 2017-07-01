﻿namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Default dependency sorter
    /// </summary>
    public class DependencySorter : IDependencySorter
    {
        /// <summary>
        /// Default comparer, swappable by changing DependencySorter in IStartupConfiguration
        /// </summary>
        protected virtual IComparer<IDependencyNode> DependencyComparer => new DependencyComparer();

        /// <summary>
        /// Default sorter or Types or Assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public virtual IEnumerable<IDependencyNode> Sort<T>(IEnumerable<object> nodes) where T : DependencyBaseAttribute
        {
            IList<IDependencyNode> unresolved = StartupNodes(nodes, typeof(T), DependencyComparer);
            List<IDependencyNode> resolved = new List<IDependencyNode>();
            HashSet<object> hashSet = new HashSet<object>();
            int count = unresolved.Count;
            int index = 0;

            while (unresolved.Count > 0)
            {
                IDependencyNode moduleNode = unresolved[index];

                if (hashSet.IsSupersetOf(moduleNode.Dependencies))
                {
                    resolved.Add(moduleNode);
                    hashSet.Add(moduleNode.Node);
                    unresolved.RemoveAt(index--);
                }

                if (++index >= unresolved.Count)
                {
                    if (count == unresolved.Count)
                    {
                        string names = string.Join(Environment.NewLine, unresolved.Select(x => x.FullName).ToArray());
                        throw new InvalidOperationException($"Cannot resolve dependencies for the following {typeof(T).FullName}: {names}, please check their dependencies!");
                    }

                    index = 0;
                    count = unresolved.Count;
                }
            }

            return resolved;
        }

        /// <summary>
        /// Creates a list of dependency nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="attr"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        protected static IList<IDependencyNode> StartupNodes(IEnumerable<object> nodes, Type attr, IComparer<IDependencyNode> comparer)
        {
            List<IDependencyNode> list = new List<IDependencyNode>(100);

            foreach (object node in nodes)
            {
                list.Add(ObjectFactory.Default.CreateDependencyNode(node, attr));
            }

            list.Sort(comparer);

            return list;
        }
    }
}