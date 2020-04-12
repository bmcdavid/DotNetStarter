using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ConfigureStartupCompleteTest : ILocatorConfigure
    {
        public bool FiredLocator { get; private set; }
        public bool FiredStartup { get; private set; }

        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            engine.OnLocatorStartupComplete += () => FiredLocator = true;
            engine.OnStartupComplete += () => FiredStartup = true;
        }
    }
}