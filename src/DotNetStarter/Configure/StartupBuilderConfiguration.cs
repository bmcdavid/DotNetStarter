using DotNetStarter.Abstractions;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderConfiguration : IStartupConfiguration
    {
        public StartupBuilderConfiguration(StartupBuilderObjectFactory objectFactory, IEnumerable<Assembly> assemblies, IStartupEnvironment environment)
        {
            Assemblies = assemblies;
            AssemblyFilter = objectFactory.CreateAssemblyFilter();
            AssemblyScanner = objectFactory.CreateAssemblyScanner();
            DependencyFinder = objectFactory.CreateDependencyFinder();
            DependencySorter = objectFactory.CreateDependencySorter();
            Environment = environment;
            Logger = objectFactory.CreateStartupLogger();
            ModuleFilter = objectFactory.CreateModuleFilter();
            TimedTaskManager = objectFactory.CreateTimedTaskManager();
            RegistrationLifecycleModifier = objectFactory.OverrideExpression.RegistrationLifecycleModifier;
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

        public IRegistrationLifecycleModifier RegistrationLifecycleModifier { get; }
    }
}
