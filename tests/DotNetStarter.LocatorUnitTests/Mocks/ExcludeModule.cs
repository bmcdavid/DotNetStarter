using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    [StartupModule]
    public class ExcludeModule : IStartupModule
    {
        public void Shutdown()
        {
        }

        public void Startup(IStartupEngine engine)
        {
        }
    }
}