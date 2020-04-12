using DotNetStarter.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    [Registration(typeof(TestFooImport))]
    public class TestFooImport
    {
        public Import<TestFooImport> FooImport { get; set; }

        public DateTime DateTime { get; } = DateTime.Now;
    }
}