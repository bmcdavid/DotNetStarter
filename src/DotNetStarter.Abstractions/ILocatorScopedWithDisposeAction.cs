using System;

namespace DotNetStarter.Abstractions
{
    //todo: temp until next breaking change, then merge with ILocatorScoped

    /// <summary>
    /// Action to perform on scoped locator's disposal
    /// </summary>
    public interface ILocatorScopedWithDisposeAction
    {
        /// <summary>
        /// Dispose action to perform on ILocatorScoped disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        void OnDispose(Action disposeAction);
    }
}