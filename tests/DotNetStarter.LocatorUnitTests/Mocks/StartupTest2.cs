using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    [StartupModule]
    public class StartupTest2 : ILocatorConfigure
    {
        internal static bool _ContainerInitCompleteCalled = false;

        public void Configure(ILocatorRegistry container, ILocatorConfigureEngine engine)
        {
            engine.OnLocatorStartupComplete += Engine_OnContainerStarted;
        }

        private void Engine_OnContainerStarted()
        {
            _ContainerInitCompleteCalled = true;
        }
    }
}