using System;

namespace DotNetStarter.Abstractions
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
        /// Dispose action to perform on ILocatorScoped disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        void OnDispose(Action disposeAction);
    }
}