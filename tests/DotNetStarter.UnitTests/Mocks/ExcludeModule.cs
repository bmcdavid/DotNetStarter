using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class ExcludeModule : IStartupModule
    {
        public void Shutdown()
        {
        }

        public void Startup(IStartupEngine engine)
        {
        }
    }
}