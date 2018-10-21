using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
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