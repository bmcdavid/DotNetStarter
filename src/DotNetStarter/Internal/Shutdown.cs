namespace DotNetStarter.Internal
{
    using Abstractions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Handles shutdown process
    /// </summary>
    [Registration(typeof(IShutdownHandler), Lifecycle.Singleton)]
    public class Shutdown : IShutdownHandler
    {
        private readonly IEnumerable<IStartupModule> _StartupModules;
        private readonly IStartupContext _StartupContext;

        /// <summary>
        /// Injected constructor
        /// </summary>
        /// <param name="locator">Locator is passed to get all startup modules in a sorted manner, as some locators cannot sort while injecting</param>
        /// <param name="startupContext"></param>
        public Shutdown(ILocator locator, IStartupContext startupContext)
        {
            _StartupModules = locator.GetAll<IStartupModule>();
            _StartupContext = startupContext;
        }

        void IShutdownHandler.InvokeShutdown()
        {
            if (_StartupModules != null)
            {
                foreach (var module in _StartupModules)
                {
                    try
                    {
                        module?.Shutdown();
                    }
                    catch (Exception ex)
                    {
                        _StartupContext.Configuration.Logger.
                            LogException($"Failed to shutdown module {module.GetType().FullName}!", ex, typeof(StartupHandler), LogLevel.Error);
                    }
                }
            }
        }
    }
}