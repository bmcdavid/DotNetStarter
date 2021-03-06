﻿using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter.Configure.Expressions
{
    /// <summary>
    /// Allows ability to configure assemblies scanned during startup process
    /// </summary>
    public sealed class AssemblyExpression
    {
        internal readonly HashSet<Assembly> Assemblies = new HashSet<Assembly>();
        internal bool WithNoScanning { get; private set; }

        /// <summary>
        /// Disables assembly scanning
        /// </summary>
        /// <returns></returns>
        public AssemblyExpression WithNoAssemblyScanning()
        {
            WithNoScanning = true;
            Assemblies.Clear();
            return this;
        }

        /// <summary>
        /// Removes given assemblies from the scanning process
        /// </summary>
        /// <param name="assembliesToRemove"></param>
        /// <returns></returns>
        public AssemblyExpression RemoveAssemblies(IEnumerable<Assembly> assembliesToRemove)
        {
            if (assembliesToRemove is null) { return this; }

            foreach (var a in assembliesToRemove)
            {
                Assemblies.Remove(a);
            }

            return this;
        }

        /// <summary>
        /// Removes the assembly for the given type from the scanning process
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public AssemblyExpression RemoveAssemblyFromType(Type t)
        {
            Assemblies.Remove(t.Assembly());
            return this;
        }

        /// <summary>
        /// Adds assemblies to the scanning process
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public AssemblyExpression WithAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null) { return this; }
            AddAssemblyRange(assemblies);
            return this;
        }

        /// <summary>
        /// Adds given types assemblies to the scanning process
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public AssemblyExpression WithAssembliesFromTypes(params Type[] types)
        {
            AddAssemblyRange(types.Select(t => t.Assembly()));
            return this;
        }

        /// <summary>
        /// Adds given assembly to the scanning process
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public AssemblyExpression WithAssembly(Assembly assembly)
        {
            if (assembly is object) Assemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Adds the given type's assembly to the scanning process
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssemblyExpression WithAssemblyFromType(Type type)
        {
            Assemblies.Add(type.Assembly());
            return this;
        }

        /// <summary>
        /// Adds the given type's assembly to the scanning process
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AssemblyExpression WithAssemblyFromType<T>() => WithAssemblyFromType(typeof(T));

        /// <summary>
        /// Gets all assemblies with DotNetStarter.Abstractions.DiscoverableAssemblyAttribute, which is generally a good starting point.
        /// <para>For netstandard1.0 assemblies MUST be provided!</para>
        /// </summary>
        /// <returns></returns>
        public AssemblyExpression WithDiscoverableAssemblies(IEnumerable<Assembly> assemblies = null, Func<Assembly, Type, IEnumerable<Attribute>> attributeChecker = null)
        {
            AddAssemblyRange(GetScannableAssemblies(assemblies, attributeChecker));
            return this;
        }

        private void AddAssemblyRange(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                Assemblies.Add(assembly);
            }
        }

        /// <summary>
        /// Filters a list of assemblies for DiscoverableAssemblyAttribute.
        /// </summary>
        /// <param name="assemblies">If null, calls internal assembly loader</param>
        /// <param name="attributeChecker"></param>
        /// <returns></returns>
        public static IList<Assembly> GetScannableAssemblies(IEnumerable<Assembly> assemblies = null, Func<Assembly, Type, IEnumerable<Attribute>> attributeChecker = null)
        {
            Func<Assembly, Type, IEnumerable<Attribute>> defaultChecker = (assembly, type) => Abstractions.Internal.TypeExtensions.CustomAttribute(assembly, type, false);
            attributeChecker = attributeChecker ?? defaultChecker;
            assemblies = assemblies ?? new Internal.AssemblyLoader().GetAssemblies();
            var filteredAssemblies = assemblies.Where(x => attributeChecker(x, typeof(DiscoverableAssemblyAttribute)).Any());

            return filteredAssemblies.ToList();
        }
    }
}