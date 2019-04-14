using System.Reflection;

namespace DotNetStarter.UnitTests.Mocks
{
    internal class TestAssemblyFilter : AssemblyFilter
    {
        public override bool FilterAssembly(Assembly assembly) => base.FilterAssembly(assembly);
    }
}