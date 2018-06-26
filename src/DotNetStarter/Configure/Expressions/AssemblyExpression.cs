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

        /// <summary>
        /// Removes given assemblies from the scanning process
        /// </summary>
        /// <param name="assembliesToRemove"></param>
        /// <returns></returns>
        public AssemblyExpression RemoveAssemblies(IEnumerable<Assembly> assembliesToRemove)
        {
            if (assembliesToRemove == null) return this;

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
            if (assemblies == null) return this;
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
            if (assembly != null) Assemblies.Add(assembly);
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
        /// <para>IMPORTANT: For ASP.Net Core applications, assemblies must be provided as there is no default assembly loader!</para>
        /// </summary>
        /// <returns></returns>
        public AssemblyExpression WithDiscoverableAssemblies(IEnumerable<Assembly> assemblies = null, Func<Assembly, Type, IEnumerable<Attribute>> attributeChecker = null)
        {
            AddAssemblyRange(ApplicationContext.GetScannableAssemblies(assemblies, attributeChecker));
            return this;
        }

        private void AddAssemblyRange(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                Assemblies.Add(assembly);
            }
        }
    }
}