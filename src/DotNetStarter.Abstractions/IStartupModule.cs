namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Starup Component, important requires the [StartupModuleAttribute] class attribute as well for dependency sorting!
    /// </summary>
    public interface IStartupModule
    {
        /// <summary>
        /// Startup
        /// </summary>
        void Startup(IStartupEngine engine);

        /// <summary>
        /// Shutdown
        /// </summary>
        void Shutdown(IStartupEngine engine);
    }
}