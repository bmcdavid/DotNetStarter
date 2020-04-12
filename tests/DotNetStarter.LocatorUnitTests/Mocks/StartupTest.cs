using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    [StartupModule]
    public class StartupTest : IStartupModule
    {
        public void Shutdown()
        {
        }

        public void Startup(IStartupEngine engine)
        {
            System.Diagnostics.Debug.Write("Ran startup");
        }
    }
}