using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.UnitTests
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

    [TestClass]
    public class StartupEngineTests
    {
        [TestMethod]
        public void ShouldCallContainerInitCompleteEvent()
        {
            var x = ApplicationContext.Default.Configuration.Assemblies.ToList();

            Assert.IsTrue(StartupTest2._ContainerInitCompleteCalled);
        }

        [TestMethod]
        public void ShouldFireEventsForILocatorConfigure()
        {
            var sut = new ConfigureStartupCompleteTest();
            var builder = Configure.StartupBuilder.Create();
            builder
                .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                .ConfigureStartupModules(m => m.ConfigureLocatorModuleCollection
                    (c =>
                    {
                        c.Add(sut);
                    })
                )
                .UseTestLocator()
                .Build(useApplicationContext: false);

            Assert.IsTrue(sut.FiredLocator);
            builder.Run();
            Assert.IsTrue(sut.FiredStartup);
        }
    }
}