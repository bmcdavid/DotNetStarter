namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Scans given assemblies and stores them in a static registry for later retrieval
    /// </summary>
    public class AssemblyScanner : IAssemblyScanner
    {
        private static Dictionary<Type, HashSet<Type>> ScannedRegistry = new Dictionary<Type, HashSet<Type>>();

        /// <summary>
        /// Creates a new dictionary from scanned registry, not part of IAssemblyScanner
        /// </summary>
        public virtual Dictionary<Type, HashSet<Type>> ReviewRegistry => new Dictionary<Type, HashSet<Type>>(ScannedRegistry);

        /// <summary>
        /// Get scanned types for given type, note: it doesn't filter interfaces or abstracts. 
        /// <para>To register a type use the ScanTypeRegistryAttribute assembly attribute!</para>
        /// </summary>
        /// <param name="scannedType"></param>
        /// <returns></returns>
        public virtual IEnumerable<Type> GetTypesFor(Type scannedType)
        {
            HashSet<Type> foundTypes;
            ScannedRegistry.TryGetValue(scannedType, out foundTypes);

            return foundTypes ?? Enumerable.Empty<Type>();
        }

        /// <summary>
        /// Scan the given assemblies for types registered via ScanTypeRegistryAttribute, excludes controlled by the assemblyFilter.
        /// </summary>
        /// <param name="scanAssemblies"></param>
        /// <param name="forTypes"></param>
        /// <param name="assemblyFilter"></param>
        public virtual void Scan(IEnumerable<Assembly> scanAssemblies, IEnumerable<Type> forTypes, Func<Assembly, bool> assemblyFilter = null)
        {
            if (assemblyFilter != null)
                scanAssemblies = scanAssemblies.Where(assemblyFilter);

            var types = scanAssemblies.SelectMany(x => x.GetTypesCheck());
            var attrType = typeof(Attribute);

            foreach (var type in types)
            {
                foreach (var item in forTypes)
                {
                    bool isMatch = false;

                    if (attrType.IsAssignableFromCheck(item))
                    {
                        isMatch = type.CustomAttribute(item, false).Any();
                    }
                    else if (item.IsInterface())
                    {
                        isMatch = type.HasInterface(item);
                    }
                    else
                    {
                        isMatch = item.IsAssignableFromCheck(type);
                    }

                    if (isMatch)
                    {
                        HashSet<Type> storedTypes;

                        if (!ScannedRegistry.TryGetValue(item, out storedTypes))
                        {
                            storedTypes = new HashSet<Type>();
                        }

                        storedTypes.Add(type);
                        ScannedRegistry[item] = storedTypes;
                    }
                }
            }
        }
    }
}