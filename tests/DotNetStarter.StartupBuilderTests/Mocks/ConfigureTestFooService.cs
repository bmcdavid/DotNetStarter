using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    [StartupModule]
    public class ConfigureTestFooService : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, IStartupEngineConfigurationArgs engine)
        {
            registry.Add<TestFoo, TestFoo>();
        }
    }
}