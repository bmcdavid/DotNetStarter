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
        /// Single instance per http request, note: not all containers support this
        /// </summary>
        HttpRequest = 2,
        /// <summary>
        /// Single instance per thread, note: not all containers support this
        /// </summary>
        //Thread = 3,
        /// <summary>
        /// Single instance per container, note: not all containers support this
        /// </summary>
        //Container = 4,
        /// <summary>
        /// Single instance per scope: note: not all containers support this
        /// </summary>
        Scoped = 5,
        /// <summary>
        /// Always creates a new instance
        /// </summary>
        AlwaysUnique = 6

    }
}
