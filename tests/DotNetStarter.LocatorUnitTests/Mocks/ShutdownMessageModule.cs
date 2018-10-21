using DotNetStarter.Abstractions;
using System;
using System.Diagnostics;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class ShutdownMessageModule : IStartupModule
    {
        public void Shutdown()
        {
            Debug.WriteLine("DNS locator shutdown");
            Console.WriteLine("DNS locator shutdown");
        }

        public void Startup(IStartupEngine engine)
        {
            Debug.WriteLine("DNS Locator startup");
        }
    }
}