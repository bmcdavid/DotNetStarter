using DotNetStarter.Abstractions;
using DotNetStarter.Configure.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderObjectFactory : IStartupObjectFactory
    {
        private IStartupObjectFactory _defaultStartupObjectFactory;
        public AssemblyExpression AssemblyExpression { get; set; }
        public IStartupEnvironment Environment { private get; set; }
        public OverrideExpression OverrideExpression { get; set; }
        public StartupModulesExpression StartupModulesExpression { get; set; }
        public IAssemblyFilter CreateAssemblyFilter()
        {
            // assume if we are given assemblies and no filter, we do not need to filter
            if (OverrideExpression.AssemblyFilter == null && AssemblyExpression?.Assemblies.Count > 0) return null;

            return OverrideExpression.AssemblyFilter ?? ResolveObjectFactory().CreateAssemblyFilter();
        }

        public IAssemblyScanner CreateAssemblyScanner()
        {
            return OverrideExpression.AssemblyScanner ?? ResolveObjectFactory().CreateAssemblyScanner();
        }

        public ILocatorDefaultRegistrations CreateContainerDefaults()
        {
            return ResolveObjectFactory().CreateContainerDefaults();
        }

        public IDependencyFinder CreateDependencyFinder()
        {
            return OverrideExpression.DependencyFinder ?? ResolveObjectFactory().CreateDependencyFinder();
        }

        public IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return ResolveObjectFactory().CreateDependencyNode(nodeType, attributeType);
        }

        public IDependencySorter CreateDependencySorter()
        {
            return OverrideExpression.DependencySorter ?? ResolveObjectFactory().CreateDependencySorter();
        }

        public IStartupModuleFilter CreateModuleFilter()
        {
            return StartupModulesExpression.RemoveModuleTypes.Count > 0 ?
                new StartupBuilderModuleFilter(StartupModulesExpression.RemoveModuleTypes) :
                ResolveObjectFactory().CreateModuleFilter();
        }

        public ILocatorRegistry CreateRegistry(IStartupConfiguration startupConfiguration)
        {
            return OverrideExpression.RegistryFactory?.CreateRegistry() ?? ResolveObjectFactory().CreateRegistry(startupConfiguration);
        }

        public IRequestSettingsProvider CreateRequestSettingsProvider()
        {
            return ResolveObjectFactory().CreateRequestSettingsProvider();
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
            return ResolveObjectFactory().CreateStartupContext
            (
                locator,
                filteredModules,
                allModules,
                startupConfiguration
            );
        }

        public IStartupHandler CreateStartupHandler()
        {
            return OverrideExpression.StartupHandler ?? ResolveObjectFactory().CreateStartupHandler();
        }

        public IStartupLogger CreateStartupLogger()
        {
            return OverrideExpression.Logger ?? ResolveObjectFactory().CreateStartupLogger();
        }
        public ITimedTask CreateTimedTask()
        {
            return ResolveObjectFactory().CreateTimedTask();
        }

        public ITimedTaskManager CreateTimedTaskManager()
        {
            return OverrideExpression.TimedTaskManager ?? ResolveObjectFactory().CreateTimedTaskManager();
        }

        private IStartupObjectFactory ResolveObjectFactory()
        {
            if (_defaultStartupObjectFactory != null) return _defaultStartupObjectFactory;
            _defaultStartupObjectFactory = OverrideExpression.FallbackStartupObjectFactory ?? new StartupObjectFactory();

            return _defaultStartupObjectFactory;
        }
    }
}
