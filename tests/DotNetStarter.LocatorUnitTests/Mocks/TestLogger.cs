using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal class TestLogger : StringLogger
    {
        public TestLogger() : base(LogLevel.Error, 100000)
        {
        }
    }
}