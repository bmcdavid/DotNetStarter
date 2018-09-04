namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// StartupHandler task that can run IStartupModule isntances
    /// </summary>
    public interface IStartupTaskWithStartupModules : IStartupTask
    {
        /// <summary>
        /// If true, executes the IStartupModule instances
        /// </summary>
        bool ExecuteStartupModules { get; }
    }
}