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
            var matches = from checkType in types
                          from registerType in forTypes
                          where IsMatch(registerType, checkType)
                          select new { registerType, checkType };

            foreach (var item in matches)
            {
                HashSet<Type> storedTypes;

                if (!ScannedRegistry.TryGetValue(item.registerType, out storedTypes))
                {
                    storedTypes = new HashSet<Type>();
                }

                storedTypes.Add(item.checkType);
                ScannedRegistry[item.registerType] = storedTypes;
            }

            //foreach (var type in types)
            //{
            //    foreach (var item in forTypes)
            //    {
            //        if (IsMatch(item, type))
            //        {
            //            HashSet<Type> storedTypes;

            //            if (!ScannedRegistry.TryGetValue(item, out storedTypes))
            //            {
            //                storedTypes = new HashSet<Type>();
            //            }

            //            storedTypes.Add(type);
            //            ScannedRegistry[item] = storedTypes;
            //        }
            //    }
            //}
        }

        static Type _AttrType = typeof(Attribute);

        static bool IsMatch(Type registeredType, Type checkType)
        {
            bool isMatch = false;

            if (_AttrType.IsAssignableFromCheck(registeredType))
            {
                isMatch = checkType.CustomAttribute(registeredType, false).Any();
            }
            else if (registeredType.IsInterface())
            {
                isMatch = checkType.HasInterface(registeredType);
            }
            else
            {
                isMatch = registeredType.IsAssignableFromCheck(checkType);
            }

            return isMatch;
        }
    }
}