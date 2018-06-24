using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderConfiguration : IStartupConfiguration, IStartupDelayed
    {
        public StartupBuilderConfiguration(StartupBuilderObjectFactory fluentObjectFactory, IEnumerable<Assembly> assemblies, IStartupEnvironment startupEnvironment)
        {
            Assemblies = assemblies;
            AssemblyFilter = fluentObjectFactory.CreateAssemblyFilter();
            AssemblyScanner = fluentObjectFactory.CreateAssemblyScanner();
            DependencyFinder = fluentObjectFactory.CreateDependencyFinder();
            DependencySorter = fluentObjectFactory.CreateDependencySorter();
            Environment = startupEnvironment;
            Logger = fluentObjectFactory.CreateStartupLogger();
            ModuleFilter = fluentObjectFactory.CreateModuleFilter();
            TimedTaskManager = fluentObjectFactory.CreateTimedTaskManager();
        }

        public IEnumerable<Assembly> Assemblies { get; }
        public IAssemblyFilter AssemblyFilter { get; }
        public IAssemblyScanner AssemblyScanner { get; }
        public IDependencyFinder DependencyFinder { get; }
        public IDependencySorter DependencySorter { get; }
        public IStartupEnvironment Environment { get; }
        public IStartupLogger Logger { get; }
        public IStartupModuleFilter ModuleFilter { get; }
        public ITimedTaskManager TimedTaskManager { get; }
        public bool EnableDelayedStartup => true;
        public Action DelayedStartup { get; set; }
    }
}
