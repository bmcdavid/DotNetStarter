﻿namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default finder for Types or Assemblies for given DependencyBaseAttribute
    /// </summary>
    public class DependencyFinder : IDependencyFinder
    {
        /// <summary>
        /// Find all typs or assemblies for given attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <param name="assemblyFilter"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> Find<T>(IEnumerable<Assembly> assemblies, Func<Assembly, bool> assemblyFilter = null) where T : StartupDependencyBaseAttribute
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            if (assemblyFilter is object)
                assemblies = assemblies.Where(assemblyFilter);

            var attrType = typeof(T);

            if (typeof(StartupAssemblyDependencyBaseAttribute).IsAssignableFromCheck(attrType))
            {
                return assemblies.Where(x => x.CustomAttribute(attrType, false).Any()).OfType<object>();
            }

            var types = assemblies.SelectMany(x => x.GetTypesCheck());

            return types.Where(x => x.CustomAttribute(attrType, false).Any()).OfType<object>();
        }
    }
}