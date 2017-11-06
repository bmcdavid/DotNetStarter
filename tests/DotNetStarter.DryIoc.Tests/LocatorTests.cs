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
        public Import<IStartupContext> _Context { get; set; }

#if !STRUCTUREMAPNET35   

        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _Context.Service.Locator;
            using (var scopedLocator = (locator as ILocatorCreateScope).CreateScope())
            {
                var noScopeTest = locator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest = scopedLocator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest2 = scopedLocator.Get<ScopeTest>();

                Assert.IsTrue(scopedLocator.Parent == null);
                Assert.IsTrue(scopedLocator.Get<ILocator>() is ILocatorScoped);

                using (var nestedScope = (scopedLocator as ILocatorCreateScope).CreateScope())
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
        public void ShouldCreateScopeWithFactory()
        {
            var locator = _Context.Service.Locator;
            var factory = locator.Get<ILocatorScopeFactory>();
            var noScopeTest = locator.Get<ScopeTest>();
            System.Threading.Thread.Sleep(1);

            using (var scopedLocator = factory.CreateScope())
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

        [TestMethod]
        public void ShouldGetLocatorScopedFromAccessor()
        {
            var factory = _Context.Service.Locator.Get<ILocatorScopeFactory>();
            var scope1 = factory.CreateScope();

            Assert.IsNotNull(scope1.Get<ILocatorScopedAccessor>().CurrentScope);
            Assert.IsNull(_Context.Service.Locator.Get<ILocatorScopedAccessor>().CurrentScope);
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
        public void ShouldResolveScopedTypeWithInjectedLocator()
        {
            var locator = _Context.Service.Locator;
            var factory = locator.Get<ILocatorScopeFactory>();

            using (var scopedLocator = factory.CreateScope())
            {
                var injectionTest = scopedLocator.Get<TestLocatorInjectionTransient>();
                var injectionTest2 = scopedLocator.Get<TestLocatorInjectionScoped>();
            }
        }

        [TestMethod]
        public void ShouldResolveComplexFuncCreator()
        {
            var sut = _Context.Service.Locator.Get<Func<IInjectable, TestFuncCreationComplex>>();

            Assert.IsNotNull(sut(new TestInjectable()));
        }

#endif

        [TestMethod]
        public void ShouldImportContext()
        {
            Assert.AreEqual(_Context.Service, ApplicationContext.Default);
        }        

        [TestMethod]
        public void ShouldResolveClassWithGreedyInternalConstructor()
        {
            Assert.IsNotNull(_Context.Service.Locator.Get<Mocks.RegistrationTestGreedyInternal>());
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyPrivateConstructor()
        {
            Assert.IsNotNull(_Context.Service.Locator.Get<Mocks.RegistrationTestGreedyPrivate>());
        }

        [TestMethod]
        public void ShouldResolveClassWithStaticConstructor()
        {
            Assert.IsNotNull(_Context.Service.Locator.Get<Mocks.RegistrationTestStatic>());
        }

        [Register(typeof(ScopeTest), LifeTime.Scoped)]
        internal class ScopeTest
        {
            public ScopeTest()
            {
                TestVariable = DateTime.Now.Ticks;
            }

            public long TestVariable { get; }
        }

        [Register(typeof(TestFuncCreationComplex), LifeTime.Transient)]
        internal class TestFuncCreationComplex
        {
            public TestFuncCreationComplex(IInjectable injectionTest, IStartupConfiguration configuration, IShutdownHandler shutdownHandler)
            {

            }
        }

        [Register(typeof(TestLocatorInjectionScoped), LifeTime.Scoped)]
        internal class TestLocatorInjectionScoped
        {
            public TestLocatorInjectionScoped(ILocator locator, ILocatorScoped locatorScoped, ILocatorScopedAccessor locatorScopedAccessor)
            {
                var x = locator as ILocatorScoped;

                if (x == null || x != locatorScoped)
                    throw new Exception("Locator injection didn't work!");

                if (locatorScoped != locatorScopedAccessor.CurrentScope)
                    throw new Exception("Scopes do not match!");
            }
        }

        [Register(typeof(TestLocatorInjectionTransient), LifeTime.Scoped)]
        internal class TestLocatorInjectionTransient
        {
            public TestLocatorInjectionTransient(ILocator locator, ILocatorScoped locatorScoped)
            {
                var x = locator as ILocatorScoped;

                if (x == null || x != locatorScoped)
                    throw new Exception("Locator injection didn't work!");
            }
        }

        internal interface IInjectable { }

        internal class TestInjectable : IInjectable
        {

        }
    }
}