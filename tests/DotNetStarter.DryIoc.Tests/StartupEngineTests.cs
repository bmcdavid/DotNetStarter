using System;
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

            public void Configure(ILocatorRegistry container, IStartupEngine engine)
            {
                engine.OnLocatorStartupComplete += Engine_OnContainerStarted;
            }

            private void Engine_OnContainerStarted()
            {
                _ContainerInitCompleteCalled = true;
            }
        }

        [StartupModule]
        public class StartupTest : IStartupModule
        {
            public void Shutdown(IStartupEngine engine)
            {
            }

            public void Startup(IStartupEngine engine)
            {
                System.Diagnostics.Debug.Write("Ran startup");
            }
        }
    }
}