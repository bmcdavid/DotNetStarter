using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

#if NETSTANDARD
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
#endif
        [TestMethod]
        public void ShouldFilterAssembliesForScannableAttributeGivenNullExceptForNetstandard()
        {
            var filter = ApplicationContext.GetScannableAssemblies(assemblies: null);

            Assert.IsTrue(filter.Any());
        }

        [TestMethod]
        public void ShouldFilterGivenAssembliesForScannableAttribute()
        {
            IEnumerable<Assembly> assemblies = new List<Assembly>
            {
                TypeExtensions.Assembly(typeof(StartupModuleTests))
            };

            var filter = ApplicationContext.GetScannableAssemblies(assemblies: assemblies);

            Assert.IsTrue(filter.Count() == 1);
        }

        [TestMethod]
        public void ShouldLogExceptionGreatherThanThreshold()
        {
            var sut = new StringLogger(LogLevel.Error, 10000);
            var e = new Exception("Testing");
            sut.LogException("Test", e, typeof(StartupTest), LogLevel.Error);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));

            sut = new StringLogger(LogLevel.Error, 10000);
            sut.LogException("Test", e, typeof(StartupTest), LogLevel.Fatal);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));
        }

        [TestMethod]
        public void ShouldLogNoExceptionLessThanThreshold()
        {
            var sut = new StringLogger(LogLevel.Fatal, 10000);
            var e = new Exception("Testing");
            sut.LogException("Test", e, typeof(StartupTest), LogLevel.Error);
            Assert.IsTrue(string.IsNullOrEmpty(sut.ToString()));
        }

        [TestMethod]
        public void ShouldClearLog()
        {
            var sut = new StringLogger(LogLevel.Debug, 10);
            var e = new Exception("Testing");
            sut.LogException("Testing Clear", e, typeof(StartupTest), LogLevel.Error);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));

            sut.LogException("New log", e, typeof(StartupTest), LogLevel.Error);
            Assert.IsFalse(sut.ToString().Contains("Testing Clear"));
            Assert.IsTrue(sut.ToString().Contains("New log"));
        }

        [TestMethod]
        public void ShouldRemoveInitModule()
        {
            var allCount = ApplicationContext.Default.AllModuleTypes.Count();
            var filteredCount = ApplicationContext.Default.FilteredModuleTypes.Count();

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

            var prev = AssemblyFactoryBaseAttribute.FactoryIsAbstract;
            AssemblyFactoryBaseAttribute.FactoryIsAbstract = sut;
            var check = new LocatorRegistryFactoryAttribute(typeof(MockFactory));
            AssemblyFactoryBaseAttribute.FactoryIsAbstract = prev;

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void ShouldShutdown()
        {
            var shutdown = DotNetStarter.ApplicationContext.Default.Locator.Get<IShutdownHandler>();
            shutdown.InvokeShutdown();

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
            new StartupHandler().Startup(ApplicationContext.Default.Configuration, new NullLocatorObjectFactory(), out IStartupContext x);
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

        public void Shutdown()
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