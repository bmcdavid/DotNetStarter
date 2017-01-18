namespace DotNetStarter.Abstractions
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Timed task
    /// </summary>
    public interface ITimedTask
    {
        /// <summary>
        /// Name of task for retrieval from timed task manager
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Task to perform
        /// </summary>
        Action TimedAction { get; set; }

        /// <summary>
        /// Time it took task to execute
        /// </summary>
        Stopwatch Timer { get; set; }

        /// <summary>
        /// Only time task in debug mode, otherwise task just executes.
        /// </summary>
        bool RequireDebugMode { get; set; }

        /// <summary>
        /// Scope of task
        /// </summary>
        TimedActionScope Scope { get; set; }
    }
}
