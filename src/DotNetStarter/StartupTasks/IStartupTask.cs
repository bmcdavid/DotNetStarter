using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Task to perform by IStartupHandler
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// Prepares task for exectution
        /// </summary>
        /// <param name="taskContext"></param>
        void Prepare(StartupTaskContext taskContext);

        /// <summary>
        /// Executes task with given ITimedTaskManager
        /// </summary>
        /// <param name="timedTaskManager"></param>
        void Execute(ITimedTaskManager timedTaskManager);
    }
}