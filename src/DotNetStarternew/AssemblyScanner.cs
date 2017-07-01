namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Scans given assemblies and stores them in a scan registry for later retrieval
    /// </summary>
    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly Dictionary<Type, HashSet<Type>> ScannedRegistry;

        private readonly IEnumerable<IAssemblyScanTypeMatcher> _TypeMatchers;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssemblyScanner() : this(null) { }

        /// <summary>
        /// Injectable Constructor
        /// </summary>
        /// <param name="typeMatchers"></param>
        public AssemblyScanner(IEnumerable<IAssemblyScanTypeMatcher> typeMatchers)
        {
            ScannedRegistry = new Dictionary<Type, HashSet<Type>>();
            _TypeMatchers = typeMatchers ?? new IAssemblyScanTypeMatcher[]
            {
                new AssemblyScanAttributeMatcher(),
                new AssemblyScanInterfaceMatcher(),
                new AssemblyScanGenericInterfaceMatcher(),
                new AssemblyScanAssignableFromMatcher()
            };
        }

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
                          where _TypeMatchers.Any(x => x.IsMatch(registerType, checkType))
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
        }
    }
}