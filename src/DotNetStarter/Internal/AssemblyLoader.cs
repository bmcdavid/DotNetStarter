using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetStarter.Abstractions;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Default assembly loader
    /// </summary>
    public class AssemblyLoader : IAssemblyLoader
    {
        private static HashSet<Assembly> _LoadedAssemblies = new HashSet<Assembly>();

        private static readonly object _Lock = new object();

#if NET35 || NET40 || NET45
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
#endif

#if NET35
        /// <summary>
        /// Gets assembly files
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetAssemblyFiles()
        {
            var assemblyPath = GetAssemblyDir();
            IEnumerable<string> files = System.IO.Directory.GetFiles(assemblyPath, "*.dll")
                                                 .Concat(System.IO.Directory.GetFiles(assemblyPath, "*.exe"));

            return files;
        }
#elif NET40 || NET45
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
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD2_0
        /// <summary>
        /// Assembly loader not implemented for netstandard
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            throw new AssembliesNotConfiguredException();
        }
#else
        /// <summary>
        /// Gets application assemblies, note: in netstandard apps local builds won't always include the dlls which could lead to test/debug issues
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            if (_LoadedAssemblies.Count == 0)
            {
                lock (_Lock)
                {
                    if (_LoadedAssemblies.Count == 0)
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

                                _LoadedAssemblies.Add(assembly);
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

            return _LoadedAssemblies;
        }
#endif
    }
}