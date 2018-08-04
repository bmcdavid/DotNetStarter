using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter
{
    /// <summary>
    /// Startup exception if IStartupEngine.Locator is accessed during configuration
    /// </summary>
    public class LocatorNotConfiguredException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorNotConfiguredException() : base($"Locator cannot be accessed during {typeof(ILocatorConfigure).FullName}.{nameof(ILocatorConfigure.Configure)}!")
        {
        }
    }
}