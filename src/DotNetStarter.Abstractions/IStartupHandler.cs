namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Main hook for startup
    /// </summary>
    public interface IStartupHandler : IStartupEngine
    {
        /// <summary>
        /// Creates the startup context
        /// </summary>
        /// <param name="startupConfiguration"></param>
        /// <param name="objectFactory"></param>
        /// <param name="startupContext"></param>
        /// <returns></returns>
        bool Startup
        (
            IStartupConfiguration startupConfiguration,
            IStartupObjectFactory objectFactory,
            out IStartupContext startupContext
        );
    }
}