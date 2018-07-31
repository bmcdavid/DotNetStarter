using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    public class ManualLocatorConfigure : ILocatorConfigure
    {
        public bool Executed { get; private set; }

        public void Configure(ILocatorRegistry registry, IStartupEngineConfigurationArgs engine)
        {
            Executed = true;    
        }
    }
}