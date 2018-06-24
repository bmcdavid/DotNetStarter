using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    [StartupModule]
    public class BadConfigureModule : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}