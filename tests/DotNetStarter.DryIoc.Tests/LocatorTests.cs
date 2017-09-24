using System;
using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Abstractions.Internal;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class LocatorTests
    {
        internal class TestScopeKind : IScopeKind
        {
            public string Name => throw new NotImplementedException();

            public Type ScopeType => throw new NotImplementedException();
        }

        [Register(typeof(ScopeTest), LifeTime.Scoped)]
        public class ScopeTest
        {
            public long TestVariable { get; }

            public ScopeTest()
            {
                TestVariable = DateTime.Now.Ticks;
            }
        }

        public Import<IStartupContext> _Context { get; set; }

        [TestMethod]
        public void ShouldImportContext()
        {
            Assert.AreEqual(_Context.Service, ApplicationContext.Default);
        }

#if STRUCTUREMAPNET35
        [ExpectedException(typeof(System.NotImplementedException))]
#endif
        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _Context.Service.Locator;
            var scopedLocator = (locator as ILocatorCreateScope).CreateScope(new TestScopeKind());

            var noScopeTest = locator.Get<ScopeTest>();
            var scopeTest = scopedLocator.Get<ScopeTest>();
            System.Threading.Thread.Sleep(1);
            var scopeTest2 = scopedLocator.Get<ScopeTest>();

            Assert.IsTrue(scopedLocator.IsActiveScope);
            Assert.IsTrue(scopedLocator.Get<ILocator>() is ILocatorScoped);

            Assert.AreEqual(scopeTest.TestVariable, scopeTest2.TestVariable);
            Assert.AreNotEqual(scopeTest2.TestVariable, noScopeTest.TestVariable);

            scopedLocator.Dispose();
            var resolveTest = locator.Get<IAssemblyScanner>();
            Assert.IsNotNull(resolveTest);
        }
    }
}