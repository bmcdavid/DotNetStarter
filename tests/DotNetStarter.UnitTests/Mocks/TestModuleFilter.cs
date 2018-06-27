using System.Collections.Generic;
using System.Linq;
using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    internal class TestModuleFilter : StartupModuleFilter
    {
        public override IEnumerable<IDependencyNode> FilterModules(IEnumerable<IDependencyNode> modules)
        {
            return modules.Where(x => !x.FullName.Contains(nameof(ExcludeModule))).OrderByDescending(x => x.DependencyCount);
        }
    }
}
