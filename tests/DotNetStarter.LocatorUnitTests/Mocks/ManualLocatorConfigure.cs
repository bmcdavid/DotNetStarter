using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    public class ManualLocatorConfigure : ILocatorConfigure
    {
        public bool Executed { get; private set; }

        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            Executed = true;
        }
    }
}