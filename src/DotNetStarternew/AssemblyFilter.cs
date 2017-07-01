﻿namespace DotNetStarter
{
    using Abstractions;
    using System.Reflection;

    /// <summary>
    /// Default Assembly filter
    /// </summary>
    public class AssemblyFilter : IAssemblyFilter
    {
        /// <summary>
        /// Default filter excludes: mscorelib, system, microsoft, ektron, and episerver namespaces
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public virtual bool FilterAssembly(Assembly assembly)
        {
#if NET35 || NET40 || NET45
            if (assembly.GlobalAssemblyCache) return false;
#endif

#if NET40 || NET45 || NETSTANDARD
            if (assembly.IsDynamic) return false;
#endif

            var name = assembly.GetName().Name.Split('.');

            switch (name[0].ToLower())
            {
                case "mscorlib":
                case "system":
                case "microsoft":
                    return false;
            }

            return true;
        }
    }
}