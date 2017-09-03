﻿namespace DotNetStarter.Abstractions
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
        /// Single instance per http request
        /// </summary>
        HttpRequest = 2,
        /// <summary>
        /// Single instance per scope
        /// </summary>
        Scoped = 4,
        /// <summary>
        /// Always creates a new instance
        /// </summary>
        //todo: v2 remove AlwaysUnique
        [System.Obsolete("Specific to structuremap, will be removed on next breaking change.", false)]
        AlwaysUnique = 5
    }
}
