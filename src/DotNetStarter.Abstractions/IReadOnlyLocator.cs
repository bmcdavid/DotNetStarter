namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// A read only locator has completed the locator configure process
    /// <para> And internal container may only be set one more time, or never if EnsureLocked is called</para>
    /// </summary>
    public interface IReadOnlyLocator : ILocator
    {
        /// <summary>
        /// Locks container if not locked, safe to call multiple times
        /// </summary>
        void EnsureLocked();

        /// <summary>
        /// Returns true if a locking action has occurred.
        /// </summary>
        bool IsLocked { get; }
    }
}
