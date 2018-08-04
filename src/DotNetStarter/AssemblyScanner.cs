namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Scans given assemblies and stores them in a scan registry for later retrieval
    /// </summary>
    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly IEnumerable<IAssemblyScanTypeMatcher> _TypeMatchers;
        private readonly Dictionary<Type, HashSet<Type>> ScannedRegistry;

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
        /// <para>To register a type use the DiscoverTypesAttribute assembly attribute!</para>
        /// </summary>
        /// <param name="scannedType"></param>
        /// <returns></returns>
        public virtual IEnumerable<Type> GetTypesFor(Type scannedType)
        {
            ScannedRegistry.TryGetValue(scannedType, out HashSet<Type> foundTypes);

            return foundTypes ?? Enumerable.Empty<Type>();
        }

        /// <summary>
        /// Scan the given assemblies for types registered via DiscoverTypesAttribute, excludes controlled by the assemblyFilter.
        /// </summary>
        /// <param name="scanAssemblies"></param>
        /// <param name="forTypes"></param>
        /// <param name="assemblyFilter"></param>
        public virtual void Scan(IEnumerable<Assembly> scanAssemblies, IEnumerable<Type> forTypes, Func<Assembly, bool> assemblyFilter = null)
        {
            if (assemblyFilter != null)
                scanAssemblies = scanAssemblies.Where(x => !assemblyFilter(x));

            var types = scanAssemblies.SelectMany(x => AssemblyTypes(x));
            var matches = from checkType in types
                          from registerType in forTypes
                          where _TypeMatchers.Any(x => x.IsMatch(registerType, checkType))
                          select new { registerType, checkType };

            foreach (var item in matches)
            {
                if (!ScannedRegistry.TryGetValue(item.registerType, out HashSet<Type> storedTypes))
                {
                    storedTypes = new HashSet<Type>();
                }

                storedTypes.Add(item.checkType);
                ScannedRegistry[item.registerType] = storedTypes;
            }
        }

        /// <summary>
        /// Determines how assembly types are selected, default is all types in assembly instead of exports
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Type> AssemblyTypes(Assembly assembly)
        {
            ExportsType exportsType = ExportsType.All;
            var exportAttribute = assembly.CustomAttribute(typeof(ExportsAttribute), false)
                .OfType<ExportsAttribute>()
                .FirstOrDefault();

            if (exportAttribute != null)
            {
                exportsType = exportAttribute.ExportsType;
            }

            switch (exportsType)
            {
                case ExportsType.All:
                    return assembly.GetTypesCheck(exportedOnly: false);

                case ExportsType.ExportsOnly:
                    return assembly.GetTypesCheck(exportedOnly: true);

                case ExportsType.Specfic:
                    return exportAttribute.Exports;

                default:
                    throw new NotSupportedException("Unknown ExportsType of " + exportsType);
            }
        }
    }
}