using System;
using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class NullLocatorCheckInConfigure : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            if (engine.Locator != null)
            {
                throw new Exception("The locator cannot be accessed during configuration!");
            }
        }
    }
}
