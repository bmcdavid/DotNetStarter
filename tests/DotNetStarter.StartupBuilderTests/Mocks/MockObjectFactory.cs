using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    internal class MockObjectFactory : IStartupObjectFactory
    {
        public bool Executed { get; private set; }

        public IAssemblyFilter CreateAssemblyFilter()
        {
            return new AssemblyFilter();
        }

        public IAssemblyScanner CreateAssemblyScanner()
        {
            return new AssemblyScanner();
        }

        public ILocatorDefaultRegistrations CreateContainerDefaults()
        {
            Executed = true;
            return new Internal.ContainerDefaults();
        }

        public IDependencyFinder CreateDependencyFinder()
        {
            return new DependencyFinder();
        }

        public IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return new DependencyNode(nodeType, attributeType);
        }

        public IDependencySorter CreateDependencySorter()
        {
            return new DependencySorter((o,t) => CreateDependencyNode(o,t));
        }

        public IStartupModuleFilter CreateModuleFilter()
        {
            return new StartupModuleFilter();
        }

        public ILocatorRegistry CreateRegistry(IStartupConfiguration startupConfiguration)
        {
            return new Locators.StructureMapFactory().CreateRegistry();
        }

        public IRequestSettingsProvider CreateRequestSettingsProvider()
        {
            return new RequestSettingsProvider();
        }

        public IStartupConfiguration CreateStartupConfiguration(IEnumerable<Assembly> assemblies, IStartupEnvironment startupEnvironment)
        {
            throw new NotImplementedException();
        }

        public IStartupContext CreateStartupContext(IReadOnlyLocator locator, IEnumerable<IDependencyNode> filteredModules, IEnumerable<IDependencyNode> allModules, IStartupConfiguration startupConfiguration)
        {
            return new StartupContext(locator, allModules, filteredModules, startupConfiguration);
        }

        public IStartupHandler CreateStartupHandler()
        {
            return new StartupHandler();
        }

        public IStartupLogger CreateStartupLogger()
        {
            return new StringLogger(LogLevel.Debug);
        }

        public ITimedTask CreateTimedTask()
        {
            return new TimedTask();
        }

        public ITimedTaskManager CreateTimedTaskManager()
        {
            return new TimedTaskManager(() => CreateRequestSettingsProvider());
        }
    }
}
