using System;
using System.Linq;
using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter.UnitTests
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
            InjectionTest = injectionTest;
        }

        public IInjectable InjectionTest { get; }
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

    internal interface IInjectable { int Id { get; } }

    internal class TestInjectable : IInjectable
    {
        public int Id { get; set; }
    }
    #endregion

    [TestClass]
    public class LocatorTests
    {
        public Import<IStartupContext> _Context { get; set; }

        public Import<ILocatorScopedAccessor> ScopeAccessor { get; set; }

        private ILocatorScoped CreateScope(ILocator locator = null)
        {
            var l = locator ?? _Context.Service.Locator;

            // hack: LightInject cannot resolve scoped objects when no scope is open
            return (l as ILocatorWithCreateScope).CreateScope();
        }

        [TestMethod]
        public void ShouldResolveAll()
        {
            var sut = _Context.Service.Locator.GetAll(typeof(IStartupModule)).ToList();
            var sut2 = _Context.Service.Locator.GetAll<IStartupModule>().ToList();
            Assert.IsTrue(sut.Any());
            Assert.IsTrue(sut2.Any());
            Assert.IsTrue(sut.Count() == sut2.Count());
        }

        [TestMethod]
        public void ShouldHaveScopeAccessInScope()
        {
            using (var scope = _Context.Service.Locator.Get<ILocatorScopedFactory>().CreateScope())
            {
                Assert.IsNotNull(ScopeAccessor.Service.CurrentScope);
            }
        }

        [TestMethod]
        public void ShouldHaveAmbientLocatorInScope()
        {
            var ambientLocator = _Context.Service.Locator.Get<ILocatorAmbient>();
            Assert.AreSame(_Context.Service.Locator, ambientLocator.Current);
            Assert.IsFalse(ambientLocator.IsScoped);

            using (var scope = _Context.Service.Locator.Get<ILocatorScopedFactory>().CreateScope())
            {
                Assert.AreSame(scope, ambientLocator.Current);
                Assert.IsTrue(ambientLocator.IsScoped);
            }

            Assert.AreSame(_Context.Service.Locator, ambientLocator.Current);
            Assert.IsFalse(ambientLocator.IsScoped);
        }

        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _Context.Service.Locator;
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

                Assert.IsTrue(scopedLocator.Parent == null);

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
        public void ShouldResolveSimpleFuncCreator()
        {
            var sut = _Context.Service.Locator.Get<Func<ILocatorScopedFactory>>();
            var sut1 = sut();
            var sut2 = sut();

            Assert.IsNotNull(sut);
            Assert.AreSame(sut1, sut2);
        }

        [TestMethod]
        public void ShouldResolveComplexFuncCreator()
        {
            var injectedArg = new TestInjectable() { Id = 42 };
            var sut = _Context.Service.Locator.Get<Func<IInjectable, TestFuncCreationComplex>>();
            var created = sut(injectedArg);

            Assert.IsTrue(created.InjectionTest.Id == 42);
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