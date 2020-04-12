using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal class TestAssemblyFilter : AssemblyFilter
    {
        public override bool FilterAssembly(Assembly assembly) => base.FilterAssembly(assembly);
    }
}