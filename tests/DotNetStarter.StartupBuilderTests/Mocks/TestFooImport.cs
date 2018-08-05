using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    [Registration(typeof(TestFooImport))]
    public class TestFooImport
    {
        public Import<TestFooImport> FooImport { get; set; }

        public DateTime DateTime { get; } = DateTime.Now;
    }
}