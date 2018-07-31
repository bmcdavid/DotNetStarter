namespace DotNetStarter
{
    using Abstractions;
    using System;

    /// <summary>
    /// Default Startup Engine
    /// </summary>
    public class StartupEngine : IStartupEngine
    {
        private readonly StartupEngineConfigurationArgs _startupConfigurationEngine;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="startupConfigurationEngine"></param>
        public StartupEngine(ILocator locator, StartupEngineConfigurationArgs startupConfigurationEngine)
        {
            Locator = locator ?? throw new LocatorNotConfiguredException();
            Configuration = startupConfigurationEngine.Configuration;
            _startupConfigurationEngine = startupConfigurationEngine;
        }

        /// <summary>
        /// ILocator for IStartupEngine
        /// </summary>
        public ILocator Locator { get; }

        /// <summary>
        /// Startup configuration
        /// </summary>
        public IStartupConfiguration Configuration { get; }

        /// <summary>
        /// Actions to perform on startup complete
        /// </summary>
        public event Action OnStartupComplete
        {
            add
            {
                _startupConfigurationEngine.OnStartupComplete += value;
            }
            remove
            {
                _startupConfigurationEngine.OnStartupComplete -= value;
            }
        }

        /// <summary>
        /// Raises startup complete actions
        /// </summary>
        public void RaiseStartupComplete() => _startupConfigurationEngine.RaiseStartupComplete();
    }
}