using System;
using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Abstractions.Internal;

#if DRYNETSTANDARD || MAPNETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

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

        [Register(typeof(TestLocatorInjectionScoped), LifeTime.Scoped)]
        public class TestLocatorInjectionScoped
        {
            public TestLocatorInjectionScoped(ILocator locator, ILocatorScoped locatorScoped)
            {
                var x = locator as ILocatorScoped;

                if (x == null || x != locatorScoped)
                    throw new Exception("Locator injection didn't work!");
            }
        }

        [Register(typeof(TestLocatorInjectionTransient), LifeTime.Scoped)]
        public class TestLocatorInjectionTransient
        {
            public TestLocatorInjectionTransient(ILocator locator, ILocatorScoped locatorScoped)
            {
                var x = locator as ILocatorScoped;

                if (x == null || x != locatorScoped)
                    throw new Exception("Locator injection didn't work!");
            }
        }

        public Import<IStartupContext> _Context { get; set; }

        IScopeKind _TestScope = new TestScopeKind();

        [TestMethod]
        public void ShouldImportContext()
        {
            Assert.AreEqual(_Context.Service, ApplicationContext.Default);
        }


#if STRUCTUREMAPNET35
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
#endif
        [TestMethod]
        public void ShouldOpenMultipleScopesFromFactory()
        {
            var factory = _Context.Service.Locator.Get<IServiceScopeFactory>();

            var scope1 = factory.CreateScope();
            Assert.IsNotNull(scope1);
            scope1.Dispose();

            var scope2 = factory.CreateScope();
            Assert.IsNotNull(scope2);
            scope2.Dispose();
        }

        [TestMethod]
        public void ShouldNotResolveILocatorScopeOutsideofScope()
        {
            bool failed = false;
            var locator = _Context.Service.Locator;

            try
            {
                var x = locator.Get<ILocatorScoped>();
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

#if STRUCTUREMAPNET35
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
#endif
        [TestMethod]
        public void ShouldResolveScopedTypeWithInjectedLocator()
        {
            var locator = _Context.Service.Locator;
            var factory = locator.Get<ILocatorScopeFactory>();            

            using (var scopedLocator = factory.CreateScope(_TestScope))
            {
                var injectionTest = scopedLocator.Get<TestLocatorInjectionTransient>();
                var injectionTest2 = scopedLocator.Get<TestLocatorInjectionScoped>();
            }
        }

#if STRUCTUREMAPNET35
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
#endif
        [TestMethod]
        public void ShouldCreateScopeWithFactory()
        {
            var locator = _Context.Service.Locator;
            var factory = locator.Get<ILocatorScopeFactory>();
            var noScopeTest = locator.Get<ScopeTest>();
            System.Threading.Thread.Sleep(1);

            using (var scopedLocator = factory.CreateScope(_TestScope))
            {
                var scopeTest = scopedLocator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest2 = scopedLocator.Get<ScopeTest>();

                Assert.IsNotNull(scopedLocator);

                using (var childScope = factory.CreateChildScope(scopedLocator))
                {
                    Assert.IsNotNull(childScope.Parent, "Parent is null");
                    Assert.AreEqual(childScope.Parent, scopedLocator);
                }

                Assert.AreEqual(scopeTest.TestVariable, scopeTest2.TestVariable);
                Assert.AreNotEqual(scopeTest2.TestVariable, noScopeTest.TestVariable);
            }
        }

#if STRUCTUREMAPNET35
        [ExpectedException(typeof(System.NotImplementedException))]
#endif
        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _Context.Service.Locator;
            using (var scopedLocator = (locator as ILocatorCreateScope).CreateScope(_TestScope))
            {
                var noScopeTest = locator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest = scopedLocator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest2 = scopedLocator.Get<ScopeTest>();

                Assert.IsTrue(scopedLocator.Parent == null);
                Assert.IsTrue(scopedLocator.Get<ILocator>() is ILocatorScoped);

                using (var nestedScope = (scopedLocator as ILocatorCreateScope).CreateScope(_TestScope))
                {
                    Assert.IsNotNull(nestedScope.Parent);
                    Assert.IsNull(nestedScope.Parent.Parent);
                    Assert.AreEqual(nestedScope.Parent, scopedLocator);
                }

                Assert.AreEqual(scopeTest.TestVariable, scopeTest2.TestVariable);
                Assert.AreNotEqual(scopeTest2.TestVariable, noScopeTest.TestVariable);
            }

            var resolveTest = locator.Get<IAssemblyScanner>();
            Assert.IsNotNull(resolveTest);
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyPrivateConstructor()
        {
            Assert.IsNotNull(_Context.Service.Locator.Get<Mocks.RegistrationTestGreedyPrivate>());
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyInternalConstructor()
        {
            Assert.IsNotNull(_Context.Service.Locator.Get<Mocks.RegistrationTestGreedyInternal>());
        }

        [TestMethod]
        public void ShouldResolveClassWithStaticConstructor()
        {
            Assert.IsNotNull(_Context.Service.Locator.Get<Mocks.RegistrationTestStatic>());
        }
    }
}