using System;

namespace DotNetStarter
{
    /// <summary>
    /// Startup Exception that is thrown when no ILocator/ILocatoryRegistry is found in startup
    /// </summary>
    public class NullLocatorException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NullLocatorException() : base($"{nameof(DotNetStarter)} requires an ILocator as of version 1.x, please refer to https://bmcdavid.github.io/DotNetStarter/ilocator-setup.html for assistance!")
        {
        }
    }
}