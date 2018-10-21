using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    public class ConfigureStartupCompleteTest : ILocatorConfigure
    {
        public bool FiredLocator { get; private set; }
        public bool FiredStartup { get; private set; }

        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            engine.OnLocatorStartupComplete += () => FiredLocator = true;
            engine.OnStartupComplete += () => FiredStartup = true;
        }
    }
}