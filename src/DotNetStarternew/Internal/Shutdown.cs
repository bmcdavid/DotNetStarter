namespace DotNetStarter.Internal
{
    using Abstractions;
    using System.ComponentModel;

    /// <summary>
    /// Provides access to call module shutdown, useful for unit tests, or when finalizer isn't reliable.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Register(typeof(IShutdownHandler), LifeTime.Singleton)]
    public class Shutdown : IShutdownHandler
    {
        /// <summary>
        /// Calls default initalization handler shutdown. Mainly used for unit tests.
        /// </summary>
        public static void CallShutdown()
        {
            StartupHandler.Shutdown();
        }

        void IShutdownHandler.InvokeShutdown()
        {
            CallShutdown();
        }
    }
}