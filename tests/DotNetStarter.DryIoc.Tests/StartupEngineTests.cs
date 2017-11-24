using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.Tests
{
    #region Mocks

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
        public void Shutdown()
        {
        }

        public void Startup(IStartupEngine engine)
        {
            System.Diagnostics.Debug.Write("Ran startup");
        }
    }

    #endregion

    [TestClass]
    public class StartupEngineTests
    {
        [TestMethod]
        public void ShouldCallContainerInitCompleteEvent()
        {
            var x = DotNetStarter.ApplicationContext.Default.Configuration.Assemblies.ToList();

            Assert.IsTrue(StartupTest2._ContainerInitCompleteCalled);
        }        
    }
}