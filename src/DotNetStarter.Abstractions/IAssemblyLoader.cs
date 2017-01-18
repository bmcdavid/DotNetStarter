namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Abstracts app domain loading and assembly loading for netcoreapp
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Gets an applications referenced assemblies
        /// </summary>
        /// <returns></returns>
        IEnumerable<Assembly> GetAssemblies();
    }
}
