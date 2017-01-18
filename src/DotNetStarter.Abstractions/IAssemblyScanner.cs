namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Scans assemblies for startup process and container configuration.
    /// </summary>
    public interface IAssemblyScanner
    {
        /// <summary>
        /// Scans given assemblies for given types. Filter can be used to remove assemblies from scan.
        /// </summary>
        /// <param name="scanAssemblies"></param>
        /// <param name="forTypes"></param>
        /// <param name="assemblyFilter"></param>
        void Scan(IEnumerable<Assembly> scanAssemblies, IEnumerable<Type> forTypes, Func<Assembly, bool> assemblyFilter = null);

        /// <summary>
        /// Gets all types that derive from the scanned type.
        /// </summary>
        /// <param name="scannedType"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypesFor(Type scannedType);
    }
}