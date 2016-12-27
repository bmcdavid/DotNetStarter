using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetStarter.Abstractions;

//register this as the default configuration
[assembly: StartupObjectFactory(typeof(DotNetStarter.Tests.Mocks.TestObjectFactory))]

namespace DotNetStarter.Tests.Mocks
{
    internal class TestObjectFactory : StartupObjectFactory
    {
        public override int SortOrder => base.SortOrder + 100;

        public override IStartupLogger CreateStartupLogger() => new TestLogger();

        public override IStartupModuleFilter CreateModuleFilter() => new TestModuleFilter();

    }

    internal class TestModuleFilter : StartupModuleFilter
    {
        public override IEnumerable<IDependencyNode> FilterModules(IEnumerable<IDependencyNode> modules)
        {
            return modules.Where(x => !x.FullName.Contains(nameof(ExcludeModule))).OrderByDescending(x => x.DependencyCount);
        }
    }

    internal class TestAssemblyFilter : AssemblyFilter
    {
        public override bool FilterAssembly(Assembly assembly) => base.FilterAssembly(assembly);
    }

    internal class TestLogger : StringLogger
    {

    }

    [StartupModule]
    public class ExcludeModule : IStartupModule
    {
        public void Shutdown(IStartupEngine engine)
        {
            throw new NotImplementedException();
        }

        public void Startup(IStartupEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}
