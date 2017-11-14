using System;
using System.ComponentModel;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Container service lifetimes
    /// </summary>
    public enum LifeTime
    {
        /// <summary>
        /// New instances for most containers unless scoped
        /// </summary>
        Transient = 0,
        /// <summary>
        /// Single instance per application
        /// </summary>
        Singleton = 1,
        /// <summary>
        /// Single instance per scope
        /// </summary>
        Scoped = 4
    }
}
