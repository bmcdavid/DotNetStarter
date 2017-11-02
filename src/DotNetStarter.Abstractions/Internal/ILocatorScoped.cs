using System;

namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Represents a locator which is scoped
    /// </summary>
    public interface ILocatorScoped : ILocator, IDisposable
    {
        /// <summary>
        /// Null if direct child of unscoped
        /// </summary>
        ILocatorScoped Parent { get; }

        /// <summary>
        /// Scope Kind
        /// </summary>
        IScopeKind ScopeKind { get; }
    }
}
