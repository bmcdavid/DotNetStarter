using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Abstractions;
using System.Linq;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class StartupModuleTests
    {
        [TestMethod]
        public void ShouldStartup()
        {
            Assert.IsTrue(StartupTest.InitCalled);
        }

        [TestMethod]
        public void ShouldShutdown()
        {
            Internal.Shutdown.CallShutdown();

            Assert.IsTrue(StartupTest.ShutdownCalled);
        }

        [TestMethod]
        public void ShouldCallInitCompleteEvent()
        {
            Assert.IsTrue(StartupTest._InitCompleteCalled);
        }

        [TestMethod]
        public void ShouldRemoveInitModule()
        {
            var allCount = DotNetStarter.Context.Default.AllModuleTypes.Count();
            var filteredCount = DotNetStarter.Context.Default.FilteredModuleTypes.Count();

            Assert.AreNotEqual(allCount, filteredCount);
        }
    }

    [StartupModule]
    public class StartupTest : IStartupModule
    {        
        private static bool _InitCalled = false;

        private static volatile bool _ShutdownCalled = false;        

        internal static bool _InitCompleteCalled = false;

        internal static bool InitCalled => _InitCalled;

        internal static bool ShutdownCalled => _ShutdownCalled;

        public void Startup(IStartupEngine engine)
        {
            _InitCalled = true;            
            engine.OnStartupComplete += Engine_OnStartupComplete;
        }

        private void Engine_OnStartupComplete()
        {
            _InitCompleteCalled = true;
        }

        public void Shutdown(IStartupEngine engine)
        {
            _ShutdownCalled = true;
        }
    }
}
