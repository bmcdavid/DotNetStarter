using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class StartupEngineTests
    {
        [TestMethod]
        public void ShouldCallContainerInitCompleteEvent()
        {
            var x = _TestSetup.TestContext.Configuration.Assemblies.ToList();

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
                .Build();

            Assert.IsTrue(sut.FiredLocator, "failed to fire during locator config!");
            builder.Run();
            Assert.IsTrue(sut.FiredStartup);
        }
    }
}