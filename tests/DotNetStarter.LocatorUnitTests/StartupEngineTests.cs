using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class StartupEngineTests
    {
        [TestMethod]
        public void ShouldCallContainerInitCompleteEvent()
        {
            var x = TestSetup.TestContext.Configuration.Assemblies.ToList();

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

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldThrowArgumentNullException()
        {
            new StartupEngine(new FakeLocator(), null);
        }

        [ExpectedException(typeof(LocatorNotConfiguredException))]
        [TestMethod]
        public void ShouldThrowLocatorNotConfiguredException()
        {
            new StartupEngine(null, null);
        }

        private class FakeLocator : ILocator
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public object Get(Type serviceType, string key = null)
            {
                throw new NotImplementedException();
            }

            public T Get<T>(string key = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> GetAll<T>(string key = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> GetAll(Type serviceType, string key = null)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
    }
}