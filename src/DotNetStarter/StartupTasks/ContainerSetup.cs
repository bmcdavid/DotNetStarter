using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.StartupTasks
{
    /// <summary>
    /// Container/ILocator setup task
    /// </summary>
    public class ContainerSetup : IStartupTask
    {
        private readonly ITimedTask _timedTask;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timedTaskFactory"></param>
        public ContainerSetup(Func<ITimedTask> timedTaskFactory) => _timedTask = timedTaskFactory.Invoke();

        /// <summary>
        /// Executes container setup
        /// </summary>
        /// <param name="timedTaskManager"></param>
        public void Execute(ITimedTaskManager timedTaskManager) => timedTaskManager.Execute(_timedTask);

        /// <summary>
        /// Prepares container setup
        /// </summary>
        /// <param name="taskContext"></param>
        public void Prepare(IStartupTaskContext taskContext)
        {
            _timedTask.Name = typeof(ContainerSetup).FullName;
            _timedTask.TimedAction = () =>
            {
                var locatorRegistry = taskContext.LocatorRegistry;
                var locator = taskContext.Locator;
                var modules = taskContext.Get<StartupTaskModuleCollection>();
                var configureEngine = new LocatorConfigureEngine(taskContext.Configuration);
                taskContext.SetItem(configureEngine);
                if (locatorRegistry is null) { throw new NullLocatorException(); }
                taskContext.Get<ILocatorDefaultRegistrations>().Configure(locatorRegistry, modules.FilteredModules, taskContext.Configuration);
                var locatorRegistries = (locatorRegistry as ILocatorRegistryWithResolveConfigureModules)?.ResolveConfigureModules(modules.FilteredModules, taskContext.Configuration) ?? locator.GetAll<ILocatorConfigure>();

                ConfigureRegistry(locatorRegistry, locatorRegistries, configureEngine);

                // configure import<T> locator, only on static/default startup by default
                if (taskContext.EnableImport)
                {
                    ImportHelper.OnEnsureLocator += (() => locator);
                }

                locatorRegistry.Add(typeof(ILocator), locator);
                var startupContext = CreateStartupContext(locator, modules.FilteredModules, modules.SortedModules, taskContext.Configuration);
                locatorRegistry.Add(typeof(IStartupContext), startupContext);
                taskContext.SetItem(startupContext);

                configureEngine.RaiseLocatorSetupComplete();
                (locatorRegistry as ILocatorRegistryWithVerification)?.Verify();
                taskContext.Get<Action<ILocatorRegistry>>()?.Invoke(locatorRegistry); // run finalizer action if set by developer
            };
        }

        /// <summary>
        /// Configures ILocatorConfigure modules
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="locatorRegistries"></param>
        /// <param name="configureEngine"></param>
        protected virtual void ConfigureRegistry(ILocatorRegistry registry, IEnumerable<ILocatorConfigure> locatorRegistries, ILocatorConfigureEngine configureEngine)
        {
            foreach (var map in locatorRegistries ?? Enumerable.Empty<ILocatorConfigure>())
            {
                map.Configure(registry, configureEngine);
            }
        }

        /// <summary>
        /// Creates default startup context
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="filteredModules"></param>
        /// <param name="sortedModules"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual IStartupContext CreateStartupContext(ILocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> sortedModules, IStartupConfiguration config)
        {
            return new StartupContext(locator, sortedModules, filteredModules, config);
        }

    }
}