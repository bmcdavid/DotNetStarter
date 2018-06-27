using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    internal class TestLogger : StringLogger
    {
        public TestLogger() : base(LogLevel.Error, 100000)
        {

        }
    }
}
