using System;

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
        /// <param name="intializationConfiguration"></param>
        /// <param name="objectFactory"></param>
        /// <param name="startupContext"></param>
        /// <returns></returns>
        bool Startup
        (
            IStartupConfiguration intializationConfiguration,
            IStartupObjectFactory objectFactory,
            out IStartupContext startupContext
        );
    }

    /// <summary>
    /// Contains events for intialization
    /// </summary>
    public interface IStartupEngine : IDisposable
    {
        /// <summary>
        /// Executes when locator startup is complete
        /// </summary>
        event Action OnLocatorStartupComplete;

        /// <summary>
        /// Executes when startup has completed tasks
        /// </summary>
        event Action OnStartupComplete;

        /// <summary>
        /// Configuration reference
        /// </summary>
        IStartupConfiguration Configuration { get; }
    }
}