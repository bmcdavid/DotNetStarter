using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides create scope some information about create request
    /// </summary>
    public interface IScopeKind
    {
        /// <summary>
        /// Scope name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Scope Type
        /// </summary>
        Type ScopeType { get; }
    }
}
