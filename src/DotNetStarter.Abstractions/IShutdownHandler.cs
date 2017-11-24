namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Hook to call shutdown on startup modules for setups that have their own unloading system.
    /// </summary>
    public interface IShutdownHandler
    {
        /// <summary>
        /// Invoke modules shutdown
        /// </summary>
        void Shutdown();
    }
}
