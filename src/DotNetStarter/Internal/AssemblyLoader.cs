using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Default assembly loader
    /// </summary>
    public class AssemblyLoader : IAssemblyLoader
    {
        internal static readonly HashSet<Assembly> LoadedAssemblies = new HashSet<Assembly>();
        private static readonly object _lockObj = new object();

#if NETSTANDARD2_0
        /// <summary>
        /// Assembly loader for netstandard1.6+
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames
            (
                Microsoft.Extensions.DependencyModel.DependencyContext.Default,
                Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier()
            );

            return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
        }
#else
        /// <summary>
        /// Gets assembly dll folder
        /// </summary>
        /// <returns></returns>
        protected virtual string GetAssemblyDir()
        {
            List<string> searchPaths = new List<string>();
            AppDomainSetup info = AppDomain.CurrentDomain.SetupInformation;

            if (!string.IsNullOrEmpty(info.PrivateBinPath))
            {
                // PrivateBinPath may be a semicolon separated list of subdirectories.
                searchPaths = searchPaths.Concat(info.PrivateBinPath.Split(';')).ToList();
            }

            var assembliesPath = System.IO.Path.Combine(info.ApplicationBase, searchPaths.FirstOrDefault() ?? string.Empty);

            if (!System.IO.Directory.Exists(assembliesPath))
            {
                throw new Exception("Cannot determine assembly folder!");
            }

            return assembliesPath;
        }

        /// <summary>
        /// Gets assembly files
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetAssemblyFiles()
        {
            var assemblyPath = GetAssemblyDir();
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(assemblyPath, "*.dll")
                                                 .Concat(System.IO.Directory.EnumerateFiles(assemblyPath, "*.exe"));

            return files;
        }

        /// <summary>
        /// Gets application assemblies, note: in netstandard apps local builds won't always include the dlls which could lead to test/debug issues
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            if (LoadedAssemblies.Count == 0)
            {
                lock (_lockObj)
                {
                    if (LoadedAssemblies.Count == 0)
                    {
                        var files = GetAssemblyFiles();

                        foreach (var file in files)
                        {
                            Assembly assembly = null;

                            try
                            {
                                var fileInfo = new System.IO.FileInfo(file);
                                //assembly = Assembly.Load(AssemblyName.GetAssemblyName(file));
                                assembly = Assembly.Load(new AssemblyName(fileInfo.Name.Replace(fileInfo.Extension, string.Empty)));

                                LoadedAssemblies.Add(assembly);
                            }
                            catch (BadImageFormatException)
                            {
                                // Not a managed dll/exe
                                continue;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            return LoadedAssemblies;
        }

#endif
    }
}