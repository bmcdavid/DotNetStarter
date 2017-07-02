using System;
using System.Collections.Generic;
using System.Reflection;
using DotNetStarter.Abstractions;

#if NET35 || NET40 || NET45

using System.IO;
using System.Linq;

#endif

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Default assembly loader
    /// </summary>
    public class AssemblyLoader : IAssemblyLoader
    {
        private static HashSet<Assembly> _LoadedAssemblies = new HashSet<Assembly>();

        private static readonly object _Lock = new object();

        private static volatile bool _Loaded = false;

        
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

            var assembliesPath = Path.Combine(info.ApplicationBase, searchPaths.FirstOrDefault() ?? string.Empty);

            if (!Directory.Exists(assembliesPath))
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
            IEnumerable<string> files = Directory.GetFiles(assemblyPath, "*.dll")
                                                 .Concat(Directory.GetFiles(assemblyPath, "*.exe"));

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
            IEnumerable<string> files = Directory.EnumerateFiles(assemblyPath, "*.dll")
                                                 .Concat(Directory.EnumerateFiles(assemblyPath, "*.exe"));

            return files;
        }
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
        /// <summary>
        /// Assembly loader not implemented for netstandard
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            throw new NotImplementedException($"Please pass assemblies to {typeof(ApplicationContext).FullName}.{nameof(ApplicationContext.Startup)}!");
            //var runtimeId = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
            //var libraries = Microsoft.Extensions.DependencyModel.DependencyContextExtensions.GetRuntimeAssemblyNames(Microsoft.Extensions.DependencyModel.DependencyContext.Default, runtimeId);

            //return libraries.Select(x => Assembly.Load(new AssemblyName(x.Name)));
        }
#else
        /// <summary>
        /// Gets application assemblies, note: in netstandard apps local builds won't always include the dlls which could lead to test/debug issues
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            if (!_Loaded)
            {
                lock (_Lock)
                {
                    if (!_Loaded)
                    {
                        _Loaded = true;
                        var files = GetAssemblyFiles();

                        foreach (var file in files)
                        {
                            Assembly assembly = null;

                            try
                            {
                                var fileInfo = new FileInfo(file);
                                //assembly = Assembly.Load(AssemblyName.GetAssemblyName(file));
                                assembly = Assembly.Load(
                                    new AssemblyName(fileInfo.Name.Replace(fileInfo.Extension, string.Empty)));

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