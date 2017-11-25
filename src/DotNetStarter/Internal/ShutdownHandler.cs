namespace DotNetStarter.Internal
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Handles shutdown process
    /// </summary>
    [Registration(typeof(IShutdownHandler), Lifecycle.Singleton)]
    public sealed class ShutdownHandler : IShutdownHandler
    {
        private bool _ShutdownInvoked = false;

        private readonly ILocator _Locator;
        private readonly IEnumerable<IStartupModule> _StartupModules;
        private readonly IStartupConfiguration _StartupConfiguration;

        /// <summary>
        /// Injected constructor
        /// </summary>
        /// <param name="locator">Locator is passed to get all startup modules in a sorted manner, as some locators cannot sort while injecting</param>
        /// <param name="startupConfiguration">Used to log any errors</param>
        public ShutdownHandler(ILocator locator, IStartupConfiguration startupConfiguration)
        {
            _Locator = locator;
            _StartupModules = locator.GetAll<IStartupModule>().ToList();
            _StartupConfiguration = startupConfiguration;
        }

        /// <summary>
        /// Finalize
        /// </summary>
        ~ShutdownHandler()
        {
            InvokeShutdown();
        }

        void IShutdownHandler.Shutdown() => InvokeShutdown();

        void InvokeShutdown()
        {
            if (_ShutdownInvoked) return;

            if (_StartupModules != null)
            {
                _ShutdownInvoked = true;

                foreach (var module in _StartupModules)
                {
                    try
                    {
                        module?.Shutdown();
                    }
                    catch (Exception ex)
                    {
                        _StartupConfiguration.Logger.
                            LogException($"Failed to shutdown module {module.GetType().FullName}!", ex, typeof(ShutdownHandler), LogLevel.Error);
                    }
                }

                // Dispose root locator and backing container
                _Locator.Dispose();
            }
        }
    }
}