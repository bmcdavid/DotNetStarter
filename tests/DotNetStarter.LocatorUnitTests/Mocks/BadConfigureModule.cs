using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.UnitTests.Mocks
{
    public class BadConfigureModule : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}