using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Startup configuration for web api
    /// </summary>
    [StartupModule]
    [Obsolete]
    public class StartupWebApi : IStartupModule
    {
        /// <summary>
        /// Shutdown
        /// </summary>
        /// <param name="engine"></param>
        public void Shutdown(IStartupEngine engine) { }

        /// <summary>
        /// Register API controllers
        /// </summary>
        /// <param name="engine"></param>
        public void Startup(IStartupEngine engine) { }
    }
}
