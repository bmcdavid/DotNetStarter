using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class BadStartupModule : IStartupModule
    {
        //todo: fix
        public void Shutdown()
        {
           // throw new NotImplementedException();
        }

        public void Startup(IStartupEngine engine)
        {
//throw new NotImplementedException();
        }
    }
}