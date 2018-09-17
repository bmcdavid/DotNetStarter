using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Linq;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Assembly scanning startup task
    /// </summary>
    public class AssemblyScan : IStartupTask
    {
        private readonly ITimedTask _timedTask;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        public AssemblyScan(Func<ITimedTask> timedTaskFactory) => _timedTask = timedTaskFactory.Invoke();

        /// <summary>
        /// Executes assembly scan task
        /// </summary>
        /// <param name="timedTaskManager"></param>
        public void Execute(ITimedTaskManager timedTaskManager) => timedTaskManager.Execute(_timedTask);

        /// <summary>
        /// Prepares assembly scan task
        /// </summary>
        /// <param name="taskContext"></param>
        public void Prepare(IStartupTaskContext taskContext)
        {
            _timedTask.Name = typeof(AssemblyScan).FullName;
            _timedTask.TimedAction = () =>
            {
                var assemblies = taskContext.Configuration.Assemblies;
                var discoverTypeAttrs = assemblies.SelectMany(x => x.CustomAttribute(typeof(DiscoverTypesAttribute), false).OfType<DiscoverTypesAttribute>());
                var discoverTypes = discoverTypeAttrs.SelectMany(x => x.DiscoverTypes);
                Func<System.Reflection.Assembly, bool> assemblyFilter = null; // a custom config may set this to null

                if (taskContext.Configuration.AssemblyFilter != null)
                {
                    assemblyFilter = taskContext.Configuration.AssemblyFilter.FilterAssembly;
                }

                taskContext.Configuration.AssemblyScanner.Scan(assemblies, discoverTypes, assemblyFilter);
            };
        }
    }
}