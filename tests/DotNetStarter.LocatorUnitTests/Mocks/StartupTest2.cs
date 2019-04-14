using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
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