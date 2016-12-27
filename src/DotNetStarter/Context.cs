namespace DotNetStarter
{
    using Abstractions;

    /// <summary>
    /// Default hook into startup process, preferred to access using Import&lt;T> which call this.
    /// </summary>
    public class Context
    {
        private static readonly object _Lock = new object();

        private static bool __Started = false;

        private static IStartupContext _Default;

        private static IStartupHandler _Handler;

        private static IStartupConfiguration _Configuration;

        static Context()
        {
            if (!__Started)
            {
                lock (_Lock)
                {
                    if (!__Started)
                    {
                        var factory = ObjectFactory.Default;
                        _Handler = factory.CreateStartupHandler();
                        _Configuration = factory.CreateStartupConfiguration(ObjectFactory.Assemblies);

                        __Started = _Handler.Startup(_Configuration, factory, out _Default);
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