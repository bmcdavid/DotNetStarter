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
            Assert.IsTrue(StartupModuleTest._InitCompleteCalled);
        }

        [TestMethod]
        public void ShouldClearLog()
        {
            var sut = new StringLogger(LogLevel.Debug, 10);
            var e = new Exception("Testing");
            sut.LogException("Testing Clear", e, typeof(StartupModuleTest), LogLevel.Error);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));

            sut.LogException("New log", e, typeof(StartupModuleTest), LogLevel.Error);
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
                Abstractions.Internal.TypeExtensions.Assembly(typeof(StartupModuleTests))
            };

            var filter = DotNetStarter.Configure.Expressions.AssemblyExpression.GetScannableAssemblies(assemblies: assemblies);

            Assert.IsTrue(filter.Count() == 1);
        }

        [TestMethod]
        public void ShouldLogExceptionGreatherThanThreshold()
        {
            var sut = new StringLogger(LogLevel.Error, 10000);
            var e = new Exception("Testing");
            sut.LogException("Test", e, typeof(StartupModuleTest), LogLevel.Error);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));

            sut = new StringLogger(LogLevel.Error, 10000);
            sut.LogException("Test", e, typeof(StartupModuleTest), LogLevel.Fatal);
            Assert.IsFalse(string.IsNullOrEmpty(sut.ToString()));
        }

        [TestMethod]
        public void ShouldLogNoExceptionLessThanThreshold()
        {
            var sut = new StringLogger(LogLevel.Fatal, 10000);
            var e = new Exception("Testing");
            sut.LogException("Test", e, typeof(StartupModuleTest), LogLevel.Error);
            Assert.IsTrue(string.IsNullOrEmpty(sut.ToString()));
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
            var shutdown = DotNetStarter.ApplicationContext.Default.Locator.Get<IShutdownHandler>();
            shutdown.Shutdown();

            Assert.IsTrue(StartupModuleTest.ShutdownCalled);
        }

        [TestMethod]
        public void ShouldStartup()
        {
            Assert.IsTrue(StartupModuleTest.InitCalled);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ShouldThrowInvalidLocatoryRegistry()
        {
            var check = new LocatorRegistryFactoryAttribute(typeof(object));
        }

        [ExpectedException(typeof(NullLocatorException), AllowDerivedTypes = true)]
        [TestMethod]
        public void ShouldThrowLocatorNotConfiguredException()
        {
            var builder = Configure.StartupBuilder.Create();
            builder
                .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                .ConfigureStartupModules(m => m.ConfigureLocatorModuleCollection
                    (c =>
                    {
                        c.Add(new Mocks.NullLocatorCheckInConfigure());
                    })
                )
                .Build(useApplicationContext: false)
                .Run();
        }
        [ExpectedException(typeof(NullLocatorException))]
        [TestMethod]
        public void ShouldThrowNullLocatorExceptionInDefaultHandler()
        {
            new StartupHandler(() => new TimedTask(), null, null).ConfigureLocator(ApplicationContext.Default.Configuration);
        }

        internal class MockFactory : ILocatorRegistryFactory
        {
            public ILocatorRegistry CreateRegistry()
            {
                throw new NotImplementedException();
            }
        }
    }
}