using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Null Startup Handler during startup
    /// </summary>
    public class NullStartupHandlerException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NullStartupHandlerException() :
            base($"No {typeof(IStartupHandler).FullName} was defined during startup!")
        {
        }
    }
}
