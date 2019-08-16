using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class LocatorTests
    {
        private IStartupContext _Context => TestSetup.TestContext;

        private Import<ILocatorScopedAccessor> _ScopeAccessor;

        private ILocatorScoped CreateScope(ILocator locator = null) => ((locator ?? _Context.Locator) as ILocatorWithCreateScope).CreateScope();

        [TestMethod]
        public void ShouldResolveAll()
        {
            var sut = _Context.Locator.GetAll(typeof(IStartupModule)).ToList();
            var sut2 = _Context.Locator.GetAll<IStartupModule>().ToList();
            Assert.IsTrue(sut.Any());
            Assert.IsTrue(sut2.Any());
            Assert.IsTrue(sut.Count() == sut2.Count());
        }

        [TestMethod]
        public void ShouldHaveScopeAccessInScope()
        {
            using (var scope = _Context.Locator.Get<ILocatorScopedFactory>().CreateScope())
            {
                Assert.IsNotNull(_ScopeAccessor.Service.CurrentScope);
            }
        }

        [TestMethod]
        public void ShouldImplementIServiceProvider()
        {
            Assert.IsTrue(_Context.Locator is IServiceProvider);

            using (var scope = CreateScope())
                Assert.IsTrue(scope is IServiceProvider);
        }

        [TestMethod]
        public void ShouldHaveAmbientLocatorInScope()
        {
            var ambientLocator = _Context.Locator.Get<ILocatorAmbient>();
            Assert.AreSame(_Context.Locator, ambientLocator.Current);
            Assert.IsFalse(ambientLocator.IsScoped);

            using (var scope = _Context.Locator.Get<ILocatorScopedFactory>().CreateScope())
            {
                Assert.AreSame(scope, ambientLocator.Current);
                Assert.IsTrue(ambientLocator.IsScoped);
            }

            Assert.AreSame(_Context.Locator, ambientLocator.Current);
            Assert.IsFalse(ambientLocator.IsScoped);
        }

        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _Context.Locator;
            ScopeTest noScopeTest = null;

            using (var scope = CreateScope())
            {
                noScopeTest = locator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
            }

            using (var scopedLocator = CreateScope())
            {
                var scopeTest = scopedLocator.Get<ScopeTest>();
                System.Threading.Thread.Sleep(1);
                var scopeTest2 = scopedLocator.Get<ScopeTest>();

                Assert.IsTrue(scopedLocator.Parent is null);

                using (var nestedScope = CreateScope(scopedLocator))
                {
                    Assert.IsNotNull(nestedScope.Parent);
                    Assert.IsNull(nestedScope.Parent.Parent);
                    Assert.AreEqual(nestedScope.Parent, scopedLocator);
                }

                Assert.AreEqual(scopeTest.TestVariable, scopeTest2.TestVariable);
                Assert.AreNotEqual(scopeTest2.TestVariable, noScopeTest.TestVariable);
            }

            var resolveTest = locator.Get<Abstractions.IAssemblyScanner>();
            Assert.IsNotNull(resolveTest, "final resolve");
        }

        [TestMethod]
        public void ShouldCreateScopeWithFactory()
        {
            var locator = _Context.Locator;
            var factory = locator.Get<ILocatorScopedFactory>();
            ScopeTest noScopeTest;

            // hack: not all containers can resolve scoped objects when no scope is open
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
            var factory = _Context.Locator.Get<ILocatorScopedFactory>();
            using (var scope1 = factory.CreateScope())
            {
                var sut1 = scope1.Get<ILocatorScopedAccessor>().CurrentScope;

                Assert.IsNotNull(sut1);
            }
        }

        [TestMethod]
        public void ShouldNotResolveILocatorScopeOutsideofScope()
        {
            bool failed = false;
            var locator = _Context.Locator;

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
            var factory = _Context.Locator.Get<IServiceScopeFactory>();

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
            var locator = _Context.Locator;
            var factory = locator.Get<ILocatorScopedFactory>();

            using (var scopedLocator = factory.CreateScope())
            {
                var injectionTest = scopedLocator.Get<TestLocatorInjectionTransient>();
                var injectionTest2 = scopedLocator.Get<TestLocatorInjectionScoped>();
            }
        }

        [TestMethod]
        public void ShouldResolveSimpleFuncCreator()
        {
            var sut = _Context.Locator.Get<Func<ILocatorScopedFactory>>();
            var sut1 = sut();
            var sut2 = sut();

            Assert.IsNotNull(sut);
            Assert.AreSame(sut1, sut2);
        }

        [TestMethod]
        public void ShouldResolveComplexFuncCreator()
        {
            var injectedArg = new TestInjectable() { Id = 42 };
            var sut = _Context.Locator.Get<Func<IInjectable, TestFuncCreationComplex>>();
            var created = sut(injectedArg);

            Assert.IsTrue(created.InjectionTest.Id == 42);
        }

        [TestMethod]
        public void ShouldImportContext()
        {
           Assert.AreEqual(_Context, TestSetup.TestContext);
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyInternalConstructor()
        {
            Assert.IsNotNull(_Context.Locator.Get<Mocks.RegistrationTestGreedyInternal>());
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyPrivateConstructor()
        {
            Assert.IsNotNull(_Context.Locator.Get<Mocks.RegistrationTestGreedyPrivate>());
        }

        [TestMethod]
        public void ShouldResolveClassWithStaticConstructor()
        {
            Assert.IsNotNull(_Context.Locator.Get<Mocks.RegistrationTestStatic>());
        }
    }
}