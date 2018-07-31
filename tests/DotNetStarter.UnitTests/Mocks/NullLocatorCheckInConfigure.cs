using System;
using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
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
