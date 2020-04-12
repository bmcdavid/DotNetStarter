using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TestStartupModule : IStartupModule
    {
        public bool Executed { get; private set; }

        public void Shutdown()
        {
        }

        public void Startup(IStartupEngine engine)
        {
            Executed = true;
        }
    }
}