using DotNetStarter.Abstractions;
using System;
using System.Linq;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Module sort startup task
    /// </summary>
    public class ModuleSort : IStartupTask
    {
        private readonly ITimedTask _timedTask;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        public ModuleSort(Func<ITimedTask> timedTaskFactory) => _timedTask = timedTaskFactory.Invoke();

        /// <summary>
        /// Executes module sort
        /// </summary>
        /// <param name="timedTaskManager"></param>
        public void Execute(ITimedTaskManager timedTaskManager) => timedTaskManager.Execute(_timedTask);
        
        /// <summary>
        /// Prepares module sort
        /// </summary>
        /// <param name="taskContext"></param>
        public void Prepare(IStartupTaskContext taskContext)
        {
            _timedTask.Name = typeof(ModuleSort).FullName;
            _timedTask.TimedAction = () =>
            {
                var dependents = taskContext.Configuration.AssemblyScanner.GetTypesFor(typeof(StartupModuleAttribute)).OfType<object>();
                var sortedModules = taskContext.Configuration.DependencySorter.Sort<StartupModuleAttribute>(dependents);
                var tempFiltered = taskContext.Configuration.ModuleFilter?.FilterModules(sortedModules) ?? sortedModules;

                // ensure module order wasn't tampered with
                var filteredModules = (from i in sortedModules join o in tempFiltered on i.FullName equals o.FullName select o).ToList();

                taskContext.SetItem(new StartupTaskModuleCollection(sortedModules, filteredModules));
            };
        }
    }
}