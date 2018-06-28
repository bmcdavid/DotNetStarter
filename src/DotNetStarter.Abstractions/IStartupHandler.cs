namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Main hook for startup
    /// </summary>
    public interface IStartupHandler : IStartupEngine
    {
        // todo: change signature to returning IStartupContext and take IStartupConfiguration, implementation can inject what is needed othwerwise

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
#pragma warning disable CS0612 // Type or member is obsolete
            IStartupObjectFactory objectFactory,
#pragma warning restore CS0612 // Type or member is obsolete
            out IStartupContext startupContext
        );
    }
}