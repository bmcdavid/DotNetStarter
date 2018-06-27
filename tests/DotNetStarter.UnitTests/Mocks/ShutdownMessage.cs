using System;
using DotNetStarter.Abstractions;
using System.Diagnostics;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class ShutdownMessage : IStartupModule
    {
        public void Shutdown()
        {
            Debug.WriteLine("DNS shutdown");
            Console.WriteLine("DNS shutdown");
        }

        public void Startup(IStartupEngine engine)
        {
            Debug.WriteLine("DNS startup");
        }
    }
}
