using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class StartupModuleTests
    {
        [TestMethod]
        public void ShouldCallInitCompleteEvent()
        {
            Assert.IsTrue(StartupModuleCompleteTest._InitCompleteCalled);
        }

        [TestMethod]
        public void ShouldClearLog()
        {
            var sut = new StringLogger(LogLevel.Debug, 10);
            var e = new Exception("Testing");
            sut.LogException("Testing Clear", e, typeof(StartupModuleCompleteTest), LogLevel.Error);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));

            sut.LogException("New log", e, typeof(StartupModuleCompleteTest), LogLevel.Error);
            Assert.IsFalse(sut.ToString().Contains("Testing Clear"));
            Assert.IsTrue(sut.ToString().Contains("New log"));
        }

        [TestMethod]
        public void ShouldFilterAssembliesForScannableAttributeGivenNullExceptForNetstandard()
        {
            var filter = DotNetStarter.Configure.Expressions.AssemblyExpression.GetScannableAssemblies(assemblies: null);

            Assert.IsTrue(filter.Any());
        }

        [TestMethod]
        public void ShouldFilterGivenAssembliesForScannableAttribute()
        {
            IEnumerable<Assembly> assemblies = new List<Assembly>
            {
                Abstractions.Internal.TypeExtensions.Assembly(typeof(AssemblyFilter))
            };

            var filter = Configure.Expressions.AssemblyExpression.GetScannableAssemblies(assemblies: assemblies);

            Assert.IsTrue(filter.Count() == 1);
        }

        [TestMethod]
        public void ShouldHaveStaticKeys()
        {
            Assert.AreEqual(Keys.ScopedLocatorKeyInContext, "DotNetStarter.Abstractions.Keys.ILocator");
            Assert.AreEqual(Keys.ScopedProviderKeyInContext, "DotNetStarter.Abstractions.Keys.IServiceProvider");
        }
        [TestMethod]
        public void ShouldLogExceptionGreatherThanThreshold()
        {
            var sut = new StringLogger(LogLevel.Error, 10000);
            var e = new Exception("Testing");
            sut.LogException("Test", e, typeof(StartupModuleCompleteTest), LogLevel.Error);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));

            sut = new StringLogger(LogLevel.Error, 10000);
            sut.LogException("Test", e, typeof(StartupModuleCompleteTest), LogLevel.Fatal);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));
        }

        [TestMethod]
        public void ShouldLogNoExceptionLessThanThreshold()
        {
            var sut = new StringLogger(LogLevel.Fatal, 10000);
            var e = new Exception("Testing");
            sut.LogException("Test", e, typeof(StartupModuleCompleteTest), LogLevel.Error);
            Assert.IsTrue(string.IsNullOrEmpty(sut.ToString()));
        }

        [TestMethod]
        public void ShouldRemoveInitModule()
        {
            var allCount = TestSetup.TestContext.AllModuleTypes.Count();
            var filteredCount = TestSetup.TestContext.FilteredModuleTypes.Count();

            Assert.AreNotEqual(allCount, filteredCount);
        }

        [TestMethod]
        public void ShouldSetCustomIsAbstractCheckForAseemblyFactoryBaseAttribute()
        {
            bool test = false;
            bool sut(Type type)
            {
                test = true;

                return type.IsAbstract();
            }

            var prev = AssemblyFactoryBaseAttribute.FactoryIsAbstract;
            AssemblyFactoryBaseAttribute.FactoryIsAbstract = sut;
            var check = new LocatorRegistryFactoryAttribute(typeof(MockFactory));
            AssemblyFactoryBaseAttribute.FactoryIsAbstract = prev;

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void ShouldShutdown()
        {
            var shutdown = TestSetup.TestContext.Locator.Get<IShutdownHandler>();
            if (shutdown is Internal.ShutdownHandler sh) { sh.DisposeLocator = false; } // we don't really want to dispose container in unit test
            shutdown.Shutdown();

            Assert.IsTrue(StartupModuleCompleteTest.ShutdownCalled);
        }

        [TestMethod]
        public void ShouldStartup()
        {
            Assert.IsTrue(StartupModuleCompleteTest.InitCalled);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ShouldThrowInvalidLocatoryRegistry()
        {
            new LocatorRegistryFactoryAttribute(typeof(object));
        }

        [ExpectedException(typeof(NullLocatorException))]
        [TestMethod]
        public void ShouldThrowNullLocatorExceptionInDefaultHandler()
        {
            new StartupHandler(() => new TimedTask(), null, null, null).ConfigureLocator(TestSetup.TestContext.Configuration);
        }
    }
}