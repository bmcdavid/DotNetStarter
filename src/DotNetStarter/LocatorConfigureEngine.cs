namespace DotNetStarter
{
    using Abstractions;
    using System;

    /// <summary>
    /// Default Startup Configuration Engine
    /// </summary>
    public class LocatorConfigureEngine : ILocatorConfigureEngine
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startupConfiguration"></param>
        public LocatorConfigureEngine(IStartupConfiguration startupConfiguration)
        {
            Configuration = startupConfiguration;
        }

        /// <summary>
        /// Startup configuration
        /// </summary>
        public IStartupConfiguration Configuration { get; }

        /// <summary>
        /// Locator setup complete actions
        /// </summary>
        public event Action OnLocatorStartupComplete;

        /// <summary>
        /// Startup complete actions
        /// </summary>
        public event Action OnStartupComplete;

        /// <summary>
        /// Raises locator setup complete actions
        /// </summary>
        public void RaiseLocatorSetupComplete() => OnLocatorStartupComplete?.Invoke();

        /// <summary>
        /// Raises startup complete actions
        /// </summary>
        public void RaiseStartupComplete() => OnStartupComplete?.Invoke();
    }
}