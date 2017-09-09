using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DotNetStarter.Tests
{
    public class NullLocatorObjectFactory : StartupObjectFactory
    {
        public override ILocatorRegistry CreateRegistry(IStartupConfiguration config)
        {
            return null;
        }
    }

    [TestClass]
    public class StartupModuleTests
    {
        [TestMethod]
        public void ShouldCallInitCompleteEvent()
        {
            Assert.IsTrue(StartupTest._InitCompleteCalled);
        }

        [TestMethod]
        public void ShouldRemoveInitModule()
        {
            var allCount = DotNetStarter.ApplicationContext.Default.AllModuleTypes.Count();
            var filteredCount = DotNetStarter.ApplicationContext.Default.FilteredModuleTypes.Count();

            Assert.AreNotEqual(allCount, filteredCount);
        }

        [TestMethod]
        public void ShouldSetCustomIsAbstractCheckForAseemblyFactoryBaseAttribute()
        {
            bool test = false;
            Func<Type, bool> sut = (type) =>
            {
                test = true;

                return type.IsAbstract();
            };

            var prev = LocatorRegistryFactoryAttribute.FactoryIsAbstract;
            LocatorRegistryFactoryAttribute.FactoryIsAbstract = sut;
            var check = new LocatorRegistryFactoryAttribute(typeof(MockFactory));
            LocatorRegistryFactoryAttribute.FactoryIsAbstract = prev;

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void ShouldShutdown()
        {
            Internal.Shutdown.CallShutdown();

            Assert.IsTrue(StartupTest.ShutdownCalled);
        }

        [TestMethod]
        public void ShouldStartup()
        {
            Assert.IsTrue(StartupTest.InitCalled);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ShouldThrowInvalidLocatoryRegistry()
        {
            var check = new LocatorRegistryFactoryAttribute(typeof(object));
        }

        [ExpectedException(typeof(NullLocatorException))]
        [TestMethod]
        public void ShouldThrowNullLocatorExceptionInDefaultHandler()
        {
            IStartupContext x;
            new StartupHandler().Startup(DotNetStarter.ApplicationContext.Default.Configuration, new NullLocatorObjectFactory(), out x);
        }

        internal class MockFactory : ILocatorRegistryFactory
        {
            public ILocatorRegistry CreateRegistry()
            {
                throw new NotImplementedException();
            }
        }
    }

    [StartupModule]
    public class StartupTest : IStartupModule
    {
        internal static bool _InitCompleteCalled = false;
        private static bool _InitCalled = false;

        private static volatile bool _ShutdownCalled = false;
        internal static bool InitCalled => _InitCalled;

        internal static bool ShutdownCalled => _ShutdownCalled;

        public void Shutdown(IStartupEngine engine)
        {
            _ShutdownCalled = true;
        }

        public void Startup(IStartupEngine engine)
        {
            _InitCalled = true;
            engine.OnStartupComplete += Engine_OnStartupComplete;
        }

        private void Engine_OnStartupComplete()
        {
            _InitCompleteCalled = true;
        }
    }
}