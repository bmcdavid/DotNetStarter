using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class ConfigureTestFooService : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            registry.Add<TestFoo, TestFoo>();
        }
    }
}