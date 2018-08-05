using DotNetStarter.Abstractions;
using DotNetStarter.Configure.Expressions;
using DotNetStarter.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderObjectFactory
    {
        public AssemblyExpression AssemblyExpression { get; set; }
        public IStartupEnvironment Environment { get; set; }
        public DefaultsExpression OverrideExpression { get; set; }
        public StartupModulesExpression StartupModulesExpression { get; set; }

        public IAssemblyFilter CreateAssemblyFilter()
        {
            // assume if we are given assemblies and no filter, we do not need to filter
            if (OverrideExpression.AssemblyFilter == null && AssemblyExpression?.Assemblies.Count > 0) return null;

            return OverrideExpression.AssemblyFilter ?? new AssemblyFilter();
        }

        public IAssemblyScanner CreateAssemblyScanner()
        {
            return OverrideExpression.AssemblyScanner ?? new AssemblyScanner();
        }

        public ILocatorDefaultRegistrations CreateContainerDefaults()
        {
            var defaults = OverrideExpression.ContainerDefaults ?? new ContainerDefaults();
            defaults.LocatorConfigureModuleCollection = StartupModulesExpression.InternalConfigureModules;
            defaults.StartupModuleCollection = StartupModulesExpression.InternalStartupModules;

            return defaults;
        }

        public IDependencyFinder CreateDependencyFinder()
        {
            return OverrideExpression.DependencyFinder ?? new DependencyFinder();
        }

        public IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return new DependencyNode(nodeType, attributeType);
        }

        public IDependencySorter CreateDependencySorter()
        {
            return OverrideExpression.DependencySorter ?? new DependencySorter(CreateDependencyNode);
        }

        public IStartupModuleFilter CreateModuleFilter()
        {
            return new StartupBuilderModuleFilter(StartupModulesExpression.RemoveModuleTypes);
        }

        public ILocatorRegistryFactory CreateRegistryFactory(IStartupConfiguration config)
        {
            return OverrideExpression.RegistryFactory ??
                GetAssemblyFactory<LocatorRegistryFactoryAttribute, ILocatorRegistryFactory>(config);
        }

        public IRequestSettingsProvider CreateRequestSettingsProvider()
        {
            return OverrideExpression.RequestSettingsProviderFactory?.Invoke() ?? new RequestSettingsProvider();
        }

        public IStartupConfiguration CreateStartupConfiguration(IEnumerable<Assembly> assemblies)
        {
            var environment = Environment ?? new StartupEnvironment("Local", string.Empty);
            var config = new StartupBuilderConfiguration(this, assemblies, environment);

            return config;
        }

        public IStartupContext CreateStartupContext(ILocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> allModules,
            IStartupConfiguration startupConfiguration)
        {
            return new StartupContext(locator, allModules, filteredModules, startupConfiguration);
        }

        public IStartupLogger CreateStartupLogger()
        {
            return OverrideExpression.Logger ?? new StringLogger(LogLevel.Error, 1024000);
        }

        public ITimedTask CreateTimedTask()
        {
            return OverrideExpression.TimedTaskFactory?.Invoke() ?? new TimedTask();
        }

        public ITimedTaskManager CreateTimedTaskManager()
        {
            return OverrideExpression.TimedTaskManager ?? new TimedTaskManager(CreateRequestSettingsProvider);
        }

        public Action<ILocatorRegistry> GetRegistryFinalizer()
        {
            return OverrideExpression.RegistryFinalizer ?? new Action<ILocatorRegistry>(registry => { });
        }

        private static TFactoryType GetAssemblyFactory<TFactoryAttr, TFactoryType>(IStartupConfiguration config) where TFactoryAttr : AssemblyFactoryBaseAttribute
        {
            var dependents = config.DependencyFinder.Find<TFactoryAttr>(config.Assemblies);
            var sorted = config.DependencySorter.Sort<TFactoryAttr>(dependents);

            if (!(sorted.LastOrDefault()?.NodeAttribute is AssemblyFactoryBaseAttribute attr))
                return default(TFactoryType);

            return (TFactoryType)Activator.CreateInstance(attr.FactoryType);
        }
    }
}