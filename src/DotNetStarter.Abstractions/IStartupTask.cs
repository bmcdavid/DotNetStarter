namespace DotNetStarter.Abstractions
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
        void Prepare(IStartupTaskContext taskContext);

        /// <summary>
        /// Executes task with given ITimedTaskManager
        /// </summary>
        /// <param name="timedTaskManager"></param>
        void Execute(ITimedTaskManager timedTaskManager);
    }
}