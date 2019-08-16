using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class BadConfigureModule : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            //todo: fix
           // throw new NotImplementedException();
        }
    }
}