namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Configure;
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

        /// <summary>
        /// Used to determine if application default startup has executed
        /// </summary>
        public static bool Started { get; private set; }

        private static bool _Starting = false;

        private static IStartupContext _Default;

        private ApplicationContext()
        {
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

        /// <summary>
        /// Default context instance
        /// </summary>
        public static IStartupContext Default
        {
            get
            {
                if (_Default == null)
                {
                    Configure.StartupBuilder.Create().Run();
                }

                return _Default;
            }
        }

        internal static void EnsureStartup(StartupBuilderObjectFactory factory, IStartupEnvironment environment = null, IStartupConfiguration configuration = null, IEnumerable<Assembly> assemblies = null)
        {
            if (!Started)
            {
                if (_Starting)
                {
                    throw new Exception($"Do not access {typeof(ApplicationContext).FullName}.{nameof(Default)} during startup!");
                }

                lock (_Lock)
                {
                    if (!Started)
                    {
                        _Starting = true;
                        var assembliesForStartup = configuration?.Assemblies ?? assemblies ?? new Internal.AssemblyLoader().GetAssemblies();
                        var startupConfig = configuration ?? factory.CreateStartupConfiguration(assembliesForStartup, environment);
                        _Default = RunStartup(factory, startupConfig);
                        Started = _Default != null;
                        _Starting = false;
                    }
                }
            }
        }

        internal static IStartupContext RunStartup(StartupBuilderObjectFactory objFactory, IStartupConfiguration startupConfiguration)
        {
            if (objFactory == null) throw new ArgumentNullException(nameof(objFactory));
            if (startupConfiguration == null) throw new ArgumentNullException(nameof(startupConfiguration));
            var handler = new StartupHandler(objFactory.CreateTimedTask, objFactory.CreateRegistry(startupConfiguration), objFactory.CreateContainerDefaults());

            return handler.Startup(startupConfiguration);
        }

        internal static TFactoryType GetAssemblyFactory<TFactoryAttr, TFactoryType>(IStartupConfiguration config) where TFactoryAttr : AssemblyFactoryBaseAttribute
        {
            var dependents = config.DependencyFinder.Find<TFactoryAttr>(config.Assemblies);
            var sorted = config.DependencySorter.Sort<TFactoryAttr>(dependents);

            if (!(sorted.LastOrDefault()?.NodeAttribute is AssemblyFactoryBaseAttribute attr))
                return default(TFactoryType);

            return (TFactoryType)Activator.CreateInstance(attr.FactoryType);
        }
    }
}