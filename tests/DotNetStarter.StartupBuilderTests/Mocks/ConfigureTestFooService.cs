using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    [StartupModule]
    public class ConfigureTestFooService : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            registry.Add<TestFoo, TestFoo>();
        }
    }
}