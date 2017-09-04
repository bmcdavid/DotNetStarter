using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Occurs if locator internal container is tried to be modified after locking
    /// </summary>
    public class LocatorLockedException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorLockedException() : base("Locator is locked; changes can no longer be made!") { }
    }
}
