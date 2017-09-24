using System;
using System.ComponentModel;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Constructor type
    /// </summary>
    public enum ConstructorType
    {
        /// <summary>
        /// Constructor with most parameters
        /// </summary>
        Greediest = 0,
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        [Obsolete("Please always use Greediest instead.", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#if !NETSTANDARD1_0 && !NETSTANDARD1_1
        [Browsable(false)]
#endif
        Empty = 1,
        /// <summary>
        /// Constructor with resolved parameters
        /// </summary>
        [Obsolete("Please always use Greediest instead.", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#if !NETSTANDARD1_0 && !NETSTANDARD1_1
        [Browsable(false)]
#endif
        Resolved = 2
    }
}
