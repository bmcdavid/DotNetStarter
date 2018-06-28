using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
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