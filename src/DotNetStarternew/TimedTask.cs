namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Default timed task
    /// </summary>
    public class TimedTask : ITimedTask
    {
        /// <summary>
        /// Name of task
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Determines if timer is used
        /// </summary>
        public virtual bool RequireDebugMode { get; set; }

        /// <summary>
        /// Task scope
        /// </summary>
        public virtual TimedActionScope Scope { get; set; }

        /// <summary>
        /// Task to execute
        /// </summary>
        public virtual Action TimedAction { get; set; }

        /// <summary>
        /// Time to execute task
        /// </summary>
        public virtual Stopwatch Timer { get; set; }
    }
}
