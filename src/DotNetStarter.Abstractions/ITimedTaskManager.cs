namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Executes and stores timed tasks for analyzing
    /// </summary>
    public interface ITimedTaskManager
    {
        /// <summary>
        /// Gets a timed task by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ITimedTask Get(string name);

        /// <summary>
        /// Gets all timed tasks that start with prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        IEnumerable<ITimedTask> GetAll(string prefix);

        /// <summary>
        /// Executes a timed task and stores its result.
        /// </summary>
        /// <param name="task"></param>
        void Execute(ITimedTask task);
    }
}