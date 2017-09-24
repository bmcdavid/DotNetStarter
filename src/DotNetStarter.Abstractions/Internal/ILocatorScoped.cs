using System;

namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Represents a locator which is scoped
    /// </summary>
    public interface ILocatorScoped : ILocator, IDisposable
    {
        /// <summary>
        /// Should always be true in a scoped container
        /// </summary>
        bool IsActiveScope { get; }
    }
}
