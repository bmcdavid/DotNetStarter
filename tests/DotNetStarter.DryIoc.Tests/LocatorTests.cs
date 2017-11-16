using System;
using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if DRYNETSTANDARD || MAPNETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter.Tests
{
    #region Mocks

    [Registration(typeof(ScopeTest), Lifecycle.Scoped)]
    internal class ScopeTest
    {
        public ScopeTest()
        {
            TestVariable = DateTime.Now.Ticks;
        }

        public long TestVariable { get; }
    }

    [Registration(typeof(TestFuncCreationComplex), Lifecycle.Transient)]
    internal class TestFuncCreationComplex
    {
        public TestFuncCreationComplex(IInjectable injectionTest, IStartupConfiguration configuration, IShutdownHandler shutdownHandler)
        {

        }
    }

    [Registration(typeof(TestLocatorInjectionScoped), Lifecycle.Scoped)]
    internal class TestLocatorInjectionScoped
    {
        public TestLocatorInjectionScoped(ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (null == locatorScopedAccessor.CurrentScope)
                throw new Exception("Scope not set!");
        }
    }

    [Registration(typeof(TestLocatorInjectionTransient), Lifecycle.Scoped)]
    internal class TestLocatorInjectionTransient
    {
        public TestLocatorInjectionTransient(ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (null == locatorScopedAccessor.CurrentScope)
                throw new Exception("Scope not set!");
        }
    }

    internal interface IInjectable { }

    [Registration(typeof(IInjectable))] // hack: must be registered for LightInject
    internal class TestInjectable : IInjectable
    {

    }
    #endregion

    [TestClass]
    public class LocatorTests
    {
        public Import<IStartupContext> _Context { get; set; }

        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _Context.Service.Locator;
            ScopeTest noScopeTest = null;

            // hack: LightInject cannot resolve scoped objects when no scope is open
            using (var scope = (locator as ILocatorCreateScope).CreateScope())
            {
                noScopeTest = locator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
            }

            using (var scopedLocator = (locator as ILocatorCreateScope).CreateScope())
            {
                var scopeTest = scopedLocator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest2 = scopedLocator.Get<ScopeTest>();

                Assert.IsTrue(scopedLocator.Parent == null);

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
            Assert.IsNotNull(resolveTest, "final resolve");
        }
        
        [TestMethod]
        public void ShouldCreateScopeWithFactory()
        {
            var locator = _Context.Service.Locator;
            var factory = locator.Get<ILocatorScopedFactory>();
            ScopeTest noScopeTest;

            // hack: LightInject cannot resolve scoped objects when no scope is open
            using (var scopedLocator = factory.CreateScope())
            {
                noScopeTest = locator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
            }

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
            var factory = _Context.Service.Locator.Get<ILocatorScopedFactory>();
            using (var scope1 = factory.CreateScope())
            {
                var sut1 = scope1.Get<ILocatorScopedAccessor>().CurrentScope;

                Assert.IsNotNull(sut1);
            }

            // hack : cannot resolve in LightInject
            //var sut2 = _Context.Service.Locator.Get<ILocatorScopedAccessor>().CurrentScope;
            //Assert.IsNull(sut2);
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
            var factory = locator.Get<ILocatorScopedFactory>();

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
    }
}