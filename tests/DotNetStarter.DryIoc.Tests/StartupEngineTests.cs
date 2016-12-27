using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class StartupEngineTests
    {
        [TestMethod]
        public void ShouldCallContainerInitCompleteEvent()
        {
            Assert.IsTrue(StartupTest2._ContainerInitCompleteCalled);
        }

        [StartupModule]
        public class StartupTest2 : ILocatorConfigure
        {
            internal static bool _ContainerInitCompleteCalled = false;

            public void Startup(IStartupEngine engine)
            {

            }

            public void Shutdown(IStartupEngine engine)
            {

            }

            public void Configure(ILocatorRegistry container, IStartupEngine engine)
            {
                engine.OnLocatorStartupComplete += Engine_OnContainerStarted;
            }

            private void Engine_OnContainerStarted()
            {
                _ContainerInitCompleteCalled = true;
            }
        }
    }
}