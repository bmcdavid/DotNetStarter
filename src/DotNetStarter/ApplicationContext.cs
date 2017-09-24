namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default hook into startup process, execute DotNetStarter.ApplicationContext.Startup to invoke startup.
    /// <para>preferred to access using Import&lt;T> instead of DotNetStarter.ApplicationContext.Default.Locator</para>
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

        private static IStartupConfiguration _Configuration;

        /// <summary>
        /// Finalizer
        /// </summary>
        ~ApplicationContext()
        {
            _Handler?.Dispose();
            _Default?.Dispose();
        }

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
        /// <param name="assemblies">Please do not pass assemblies if also passing a IStartupConfiguration.</param>
        public static void Startup(IStartupConfiguration configuration = null, IStartupObjectFactory objectFactory = null, IEnumerable<Assembly> assemblies = null)
        {
            EnsureStartup(configuration, objectFactory, assemblies);
        }

        /// <summary>
        /// Default context instance, swapped out by ObjectFactory.CreateStartupHandler();
        /// </summary>
        public static IStartupContext Default
        {
            get
            {
                if (_Default == null)
                    EnsureStartup();

                return _Default;
            }
        }

        static void EnsureStartup(IStartupConfiguration configuration = null, IStartupObjectFactory objectFactory = null, IEnumerable<Assembly> assemblies = null)
        {
            if (!_Started)
            {
                lock (_Lock)
                {
                    if (!_Started)
                    {
                        if (configuration?.Assemblies != null && assemblies != null)
                        {
                            throw new ArgumentException($"{nameof(configuration)} and {nameof(assemblies)} were both set, please pass configuration only in these cases.");
                        }

                        ObjectFactory.EnsureDefaultObjectFactory(configuration?.Assemblies ?? assemblies, objectFactory);
                        var factory = objectFactory ?? ObjectFactory.Default;
                        _Handler = factory.CreateStartupHandler();
                        _Configuration = configuration ?? factory.CreateStartupConfiguration(assemblies ?? ObjectFactory.Assemblies);
                        _Started = _Handler.Startup(_Configuration, factory, out _Default);
                    }
                }
            }
        }
    }
}