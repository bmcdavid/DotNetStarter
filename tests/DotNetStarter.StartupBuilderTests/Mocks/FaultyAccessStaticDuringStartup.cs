using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    public class FaultyAccessStaticDuringStartup : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            //var x = DotNetStarter.ApplicationContext.Default;
        }
    }
}