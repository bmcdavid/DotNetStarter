using DotNetStarter.Abstractions;
using DotNetStarter.Configure.Expressions;
using DotNetStarter.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderObjectFactory
    {
        public AssemblyExpression AssemblyExpression { get; set; }
        public IStartupEnvironment Environment { get; set; }
        public OverrideExpression OverrideExpression { get; set; }
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

        public ILocatorRegistry CreateRegistry(IStartupConfiguration config)
        {
            return OverrideExpression.RegistryFactory?.CreateRegistry() ??
                ApplicationContext.GetAssemblyFactory<LocatorRegistryFactoryAttribute, ILocatorRegistryFactory>(config)?.CreateRegistry();
        }

        public IRequestSettingsProvider CreateRequestSettingsProvider()
        {
            return OverrideExpression.RequestSettingsProviderFactory?.Invoke() ?? new RequestSettingsProvider();
        }

        public IStartupConfiguration CreateStartupConfiguration(IEnumerable<Assembly> assemblies, IStartupEnvironment startupEnvironment)
        {
            var environment = Environment ?? startupEnvironment ?? new StartupEnvironment("Local", string.Empty);
            var assemblyArg = AssemblyExpression.Assemblies.Count > 0 ? AssemblyExpression.Assemblies : assemblies;
            var config = new StartupBuilderConfiguration(this, assemblyArg, environment);

            return config;
        }

        public IStartupContext CreateStartupContext(IReadOnlyLocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> allModules,
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
    }
}