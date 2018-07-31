using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    [Registration(typeof(TestFooImport))]
    public class TestFooImport
    {
        public Import<TestFooImport> FooImport { get; set; }
    }
}