namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default hook into startup process, execute DotNetStarter.ApplicationContext.Startup to invoke startup.
    /// </summary>
    public class ApplicationContext
    {
        /// <summary>
        /// Dictionary Key to retrive scoped IServiceProvider
        /// </summary>
        public static readonly string ScopedProviderKeyInContext = typeof(ApplicationContext).FullName + "." + nameof(IServiceProvider);

        /// <summary>
        /// Dictionary Key to retrive scoped ILocator
        /// </summary>
        public static readonly string ScopedLocatorKeyInContext = typeof(ApplicationContext).FullName + "." + nameof(ILocator);

        private static readonly object _Lock = new object();

        private static bool _Started = false;

        private static IStartupContext _Default;

        private static IStartupHandler _Handler;

        private ApplicationContext() { }

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

        /// <summary>
        /// Entry point for startup process
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="objectFactory"></param>
        public static void Startup(IStartupConfiguration configuration, IStartupObjectFactory objectFactory = null)
        {
            EnsureStartup(configuration: configuration, objectFactory: objectFactory);
        }

        /// <summary>
        /// Entry point for startup process
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="assemblies"></param>
        /// <param name="objectFactory"></param>
        public static void Startup(IStartupEnvironment environment = null, IEnumerable<Assembly> assemblies = null, IStartupObjectFactory objectFactory = null)
        {
            EnsureStartup(environment: environment, objectFactory: objectFactory, assemblies: assemblies);
        }

        /// <summary>
        /// Default context instance, swapped out by ObjectFactory.CreateStartupHandler();
        /// </summary>
        public static IStartupContext Default
        {
            get
            {
                if (_Default == null)
                {
                    EnsureStartup();
                }

                return _Default;
            }
        }

        static void EnsureStartup(IStartupEnvironment environment = null, IStartupConfiguration configuration = null, IStartupObjectFactory objectFactory = null, IEnumerable<Assembly> assemblies = null)
        {
            if (!_Started)
            {
                lock (_Lock)
                {
                    if (!_Started)
                    {
                        var assembliesForStartup = configuration?.Assemblies ?? assemblies ?? new Internal.AssemblyLoader().GetAssemblies();
                        var factory = objectFactory ?? new StartupObjectFactory();
                        _Handler = factory.CreateStartupHandler();
                        var startupConfig = configuration ?? factory.CreateStartupConfiguration(assembliesForStartup, environment);
                        _Started = _Handler.Startup(startupConfig, factory, out _Default);
                    }
                }
            }
        }
    }
}