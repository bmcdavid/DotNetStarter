namespace DotNetStarter.Abstractions
{
    using System.Reflection;

    /// <summary>
    /// Determines assemblies to scan for types
    /// </summary>
    public interface IAssemblyFilter
    {
        /// <summary>
        /// Determines if assembly is eligble to be scanned, return true to filter, false allows scanning.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        bool FilterAssembly(Assembly assembly);
    }
}