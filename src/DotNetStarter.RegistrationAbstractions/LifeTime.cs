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
        /// Single instance per http request
        /// <para>Please use Scoped option instead</para>
        /// </summary>
        [Obsolete("Please use scoped instead", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#if !NETSTANDARD1_0 && !NETSTANDARD1_1
        [Browsable(false)]
#endif
        HttpRequest = 2,
        /// <summary>
        /// Single instance per scope
        /// </summary>
        Scoped = 4,
        /// <summary>
        /// Always creates a new instance
        /// <para>Please use Transient instead.</para>
        /// </summary>
        //note: to avoid a breaking change, this will be hidden instead with information on usage.
        [Obsolete("Specific to structuremap, please use transient instead.", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#if !NETSTANDARD1_0 && !NETSTANDARD1_1
        [Browsable(false)]
#endif
        AlwaysUnique = 5
    }
}
