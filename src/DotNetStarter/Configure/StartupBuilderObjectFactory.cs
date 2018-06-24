using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderObjectFactory : IStartupObjectFactory
    {
        public OverrideExpression OverrideExpression { get; set; }

        public AssemblyExpression AssemblyExpression { get; set; }

        public StartupModulesExpression StartupModulesExpression { get; set; }

        private readonly IStartupObjectFactory _defaultStartupObjectFactory;

        public IStartupEnvironment Environment { private get; set; }

        public StartupBuilderObjectFactory()
        {
            _defaultStartupObjectFactory = new StartupObjectFactory();
        }

        public IAssemblyFilter CreateAssemblyFilter()
        {
            // assume if we are given assemblies and no filter, we do not need to filter
            if (OverrideExpression.AssemblyFilter == null && AssemblyExpression?.Assemblies.Count > 0) return null;

            return OverrideExpression.AssemblyFilter ?? _defaultStartupObjectFactory.CreateAssemblyFilter();
        }

        public IAssemblyScanner CreateAssemblyScanner()
        {
            return OverrideExpression.AssemblyScanner ?? _defaultStartupObjectFactory.CreateAssemblyScanner();
        }

        public ILocatorDefaultRegistrations CreateContainerDefaults()
        {
            return _defaultStartupObjectFactory.CreateContainerDefaults();
        }

        public IDependencyFinder CreateDependencyFinder()
        {
            return OverrideExpression.DependencyFinder ?? _defaultStartupObjectFactory.CreateDependencyFinder();
        }

        public IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return _defaultStartupObjectFactory.CreateDependencyNode(nodeType, attributeType);
        }

        public IDependencySorter CreateDependencySorter()
        {
            return OverrideExpression.DependencySorter ?? _defaultStartupObjectFactory.CreateDependencySorter();
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
            return _defaultStartupObjectFactory.CreateStartupContext
            (
                locator,
                filteredModules,
                allModules,
                startupConfiguration
            );
        }

        public IStartupLogger CreateStartupLogger()
        {
            return OverrideExpression.Logger ?? _defaultStartupObjectFactory.CreateStartupLogger();
        }

        public IStartupHandler CreateStartupHandler()
        {
            return OverrideExpression.StartupHandler ?? _defaultStartupObjectFactory.CreateStartupHandler();
        }

        public IStartupModuleFilter CreateModuleFilter()
        {
            return StartupModulesExpression.RemoveModuleTypes.Count > 0 ?
                new StartupBuilderModuleFilter(StartupModulesExpression.RemoveModuleTypes) :
                _defaultStartupObjectFactory.CreateModuleFilter();
        }

        public ILocatorRegistry CreateRegistry(IStartupConfiguration startupConfiguration)
        {
            return OverrideExpression.RegistryFactory.CreateRegistry() ?? _defaultStartupObjectFactory.CreateRegistry(startupConfiguration);
        }

        public IRequestSettingsProvider CreateRequestSettingsProvider()
        {
            return _defaultStartupObjectFactory.CreateRequestSettingsProvider();
        }

        public ITimedTask CreateTimedTask()
        {
            return _defaultStartupObjectFactory.CreateTimedTask();
        }

        public ITimedTaskManager CreateTimedTaskManager()
        {
            return OverrideExpression.TimedTaskManager ?? _defaultStartupObjectFactory.CreateTimedTaskManager();
        }
    }
}
