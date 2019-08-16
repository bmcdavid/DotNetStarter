using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    public class FaultyAccessStaticDuringStartup : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            //var x = DotNetStarter.ApplicationContext.Default;
        }
    }
}