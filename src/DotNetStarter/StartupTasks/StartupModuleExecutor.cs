using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Task for running IStartupModule instances
    /// </summary>
    public class StartupModuleExecutor : IStartupTaskWithStartupModules
    {
        /// <summary>
        /// Executes IStartupModule if true, default is true
        /// </summary>
        public bool ExecuteStartupModules => true;

        private readonly ITimedTask _timedTask;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        public StartupModuleExecutor(Func<ITimedTask> timedTaskFactory) => _timedTask = timedTaskFactory.Invoke();

        /// <summary>
        /// Runs IStartupModule instances
        /// </summary>
        /// <param name="timedTaskManager"></param>
        public void Execute(ITimedTaskManager timedTaskManager) => timedTaskManager.Execute(_timedTask);

        /// <summary>
        /// Prepares task
        /// </summary>
        /// <param name="taskContext"></param>
        public void Prepare(IStartupTaskContext taskContext)
        {
            _timedTask.Name = typeof(StartupModuleExecutor).FullName;
            _timedTask.TimedAction = () =>
            {
                var startupEngine = new StartupEngine(taskContext.Locator, taskContext.Get<LocatorConfigureEngine>());
                var modules = startupEngine.Locator.GetAll<IStartupModule>(); // resolve all startup modules for DI
                RunStartupModules(modules, startupEngine);
                startupEngine.RaiseStartupComplete();
            };
        }

        /// <summary>
        /// Startups up given IStartupModule instances
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="startupEngine"></param>
        protected virtual void RunStartupModules(IEnumerable<IStartupModule> modules, IStartupEngine startupEngine)
        {
            foreach (var x in modules ?? Enumerable.Empty<IStartupModule>())
            {
                x.Startup(startupEngine);
            }
        }
    }
}