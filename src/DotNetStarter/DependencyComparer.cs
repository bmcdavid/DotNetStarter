namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Compares nodes by dependency count then by full name if the dependencies are equal
    /// </summary>
    public class DependencyComparer : IComparer<IDependencyNode>
    {
        /// <summary>
        /// Compares dependency nodes
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IDependencyNode x, IDependencyNode y)
        {
            int num = x.DependencyCount - y.DependencyCount;

            if (num == 0)
            {
                return string.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
            }

            return num;
        }
    }
}