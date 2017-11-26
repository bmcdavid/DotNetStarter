namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Hook to call shutdown on startup modules for setups that have their own unloading system, should not be constructor injected into IStartupModule as it creates a recursive loop.
    /// </summary>
    public interface IShutdownHandler
    {
        /// <summary>
        /// Invoke modules shutdown
        /// </summary>
        void Shutdown();
    }
}
