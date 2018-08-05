namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Finds a set of nodes that contain given DependencyBaseAttribute &lt;T&gt;
    /// </summary>
    public interface IDependencyFinder
    {
        /// <summary>
        /// Scans given assemblies for given dependencybase attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <param name="assemblyFilter"></param>
        /// <returns></returns>
        IEnumerable<object> Find<T>(IEnumerable<Assembly> assemblies, Func<Assembly, bool> assemblyFilter = null) where T : StartupDependencyBaseAttribute;
    }
}