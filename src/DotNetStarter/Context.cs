namespace DotNetStarter
{
    using Abstractions;
    using System;

    /// <summary>
    /// Default hook into startup process, preferred to access using Import&lt;T> which call this.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Dictionary Key to retrive scoped IServiceProvider
        /// </summary>
        public static readonly string ScopedProviderKeyInContext = typeof(Context).FullName + "." + nameof(IServiceProvider);

        /// <summary>
        /// Dictionary Key to retrive scoped ILocator
        /// </summary>
        public static readonly string ScopedLocatorKeyInContext = typeof(Context).FullName + "." + nameof(ILocator);

        private static readonly object _Lock = new object();

        private static bool _Started = false;

        private static IStartupContext _Default;

        private static IStartupHandler _Handler;

        private static IStartupConfiguration _Configuration;

        static Context()
        {
            if (!_Started)
            {
                lock (_Lock)
                {
                    if (!_Started)
                    {
                        var factory = ObjectFactory.Default;
                        _Handler = factory.CreateStartupHandler();
                        _Configuration = factory.CreateStartupConfiguration(ObjectFactory.Assemblies);

                        _Started = _Handler.Startup(_Configuration, factory, out _Default);
                    }
                }
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Context()
        {
            _Handler?.Dispose();
            _Default?.Dispose();
        }

        private Context() { }

        /// <summary>
        /// Startup kickoff, to customize the IAssemblyLoader please execute AssemblyLoader.SetAssemblyLoader(IAssemblyLoader loader) before using Context!
        /// </summary>
        public static void Startup() { }

        /// <summary>
        /// Default context instance, swapped out by ObjectFactory.CreateStartupHandler();
        /// </summary>
        public static IStartupContext Default => _Default;
    }
}