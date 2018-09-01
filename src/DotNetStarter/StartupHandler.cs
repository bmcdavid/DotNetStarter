using DotNetStarter.Abstractions;
using DotNetStarter.StartupTasks;
using System;
using System.Collections.Generic;

namespace DotNetStarter
{
    /// <summary>
    /// Default startup handler
    /// </summary>
    public class StartupHandler : IStartupHandler
    {
        private readonly bool _enableDelayedStartupModules = false;
        private readonly Action<ILocatorRegistry> _finalizeRegistry;
        private readonly bool _enableImport;
        private readonly ILocatorDefaultRegistrations _locatorDefaultRegistrations;
        private readonly ILocatorRegistryFactory _locatorRegistryFactory;
        private readonly Func<ITimedTask> _timedTaskFactory;
        private Action _delayedStart;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        /// <param name="locatorRegistryFactory"></param>
        /// <param name="locatorDefaultRegistrations"></param>
        /// <param name="finalizeRegistry">Action for application developers to perform any last minute tasks after all other actions are performed.</param>
        /// <param name="enableDelayedStartupModules">If true, doesn't run IStartupModules until IStartupHandler.StartupModules is invoked, default is true</param>
        /// <param name="enableImport"></param>
        public StartupHandler(Func<ITimedTask> timedTaskFactory, ILocatorRegistryFactory locatorRegistryFactory, ILocatorDefaultRegistrations locatorDefaultRegistrations, Action<ILocatorRegistry> finalizeRegistry, bool enableDelayedStartupModules = true, bool enableImport = true)
        {
            _timedTaskFactory = timedTaskFactory;
            _locatorRegistryFactory = locatorRegistryFactory;
            _locatorDefaultRegistrations = locatorDefaultRegistrations;
            _enableDelayedStartupModules = enableDelayedStartupModules;
            _finalizeRegistry = finalizeRegistry;
            _enableImport = enableImport;
        }

        /// <summary>
        /// Starup process, by default it scans assemblies, sorts modules, configures container, and runs startup for each module
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual IStartupContext ConfigureLocator(IStartupConfiguration config)
        {
            IStartupTask startupTaskWithStartModules = null;
            var startupTasks = CreateStartupTasks();
            var startupTaskContext = new StartupTaskContext(_enableImport, _locatorRegistryFactory, config, _locatorDefaultRegistrations, _finalizeRegistry, new LocatorConfigureEngine(config));

            foreach (var task in startupTasks)
            {
                task.Prepare(startupTaskContext);

                if (task is IStartupTaskWithStartupModules withStartupModules && withStartupModules.ExecuteStartupModules)
                {
                    startupTaskWithStartModules = withStartupModules;
                    continue;
                }

                task.Execute(config.TimedTaskManager);
            }

            if (startupTaskWithStartModules == null)
            {
                throw new Exception("No IStartupTaskWithStartupModules was used in startup tasks!");
            }

            // optionally allows delaying startup until later, must be implemented on IStartupConfiguration instances
            _delayedStart = () => startupTaskWithStartModules.Execute(config.TimedTaskManager);

            if (!_enableDelayedStartupModules)
            {
                TryExecuteStartupModules();
            }

            return startupTaskContext.Get<IStartupContext>();
        }

        /// <summary>
        /// Tries to run IStartupModules if delayed execute is enabled
        /// </summary>
        public bool TryExecuteStartupModules()
        {
            if (_delayedStart == null) { return false; }
            _delayedStart.Invoke();
            _delayedStart = null;
            return true;
        }

        /// <summary>
        /// Creates a start task sequence, order does matter!
        /// </summary>
        /// <returns></returns>
        protected virtual ICollection<IStartupTask> CreateStartupTasks()
        {
            return new List<IStartupTask>
            {
                new AssemblyScan(_timedTaskFactory),
                new ModuleSort(_timedTaskFactory),
                new ContainerSetup(_timedTaskFactory),
                new StartupModuleExecutor(_timedTaskFactory)
            };
        }

        /// <summary>
        /// Configures ILocatorConfigure modules
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="locatorRegistries"></param>
        /// <param name="configureEngine"></param>
        [Obsolete("Use now in ContainerSetup.ConfigureRegistry!", false)]
        protected virtual void ConfigureRegistry(ILocatorRegistry registry, IEnumerable<ILocatorConfigure> locatorRegistries, ILocatorConfigureEngine configureEngine) { }

        /// <summary>
        /// Creates default startup context
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="sortedModules"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        [Obsolete("Used now in ContainerSetup.CreateStartupContext!", false)]
        protected virtual IStartupContext CreateStartupContext(ILocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> sortedModules, IStartupConfiguration config) => null;

        /// <summary>
        /// Startups up given IStartupModule instances
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="startupEngine"></param>
        [Obsolete("Use now in StartupModuleExecutorer.RunStartupModules!", false)]
        protected virtual void ExecuteStartupModules(IEnumerable<IStartupModule> modules, IStartupEngine startupEngine) { }
    }
}