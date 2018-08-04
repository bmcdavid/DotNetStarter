namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Object lifecycles
    /// </summary>
    public enum Lifecycle
    {
        /// <summary>
        /// New instances
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