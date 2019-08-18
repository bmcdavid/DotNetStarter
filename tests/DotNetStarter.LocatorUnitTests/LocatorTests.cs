using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class LocatorTests
    {
        private Import<ILocatorScopedAccessor> _scopeAccessor;
        private readonly IStartupContext _context = TestSetup.TestContext;

        [TestMethod]
        public void ShouldCreateLocatorScope()
        {
            var locator = _context.Locator;
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
            var locator = _context.Locator;
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
            var factory = _context.Locator.Get<ILocatorScopedFactory>();
            using (var scope1 = factory.CreateScope())
            {
                var sut1 = scope1.Get<ILocatorScopedAccessor>().CurrentScope;

                Assert.IsNotNull(sut1);
            }
        }

        [TestMethod]
        public void ShouldHaveAmbientLocatorInScope()
        {
            var ambientLocator = _context.Locator.Get<ILocatorAmbient>();
            Assert.AreSame(_context.Locator, ambientLocator.Current);
            Assert.IsFalse(ambientLocator.IsScoped);

            using (var scope = _context.Locator.Get<ILocatorScopedFactory>().CreateScope())
            {
                Assert.AreSame(scope, ambientLocator.Current);
                Assert.IsTrue(ambientLocator.IsScoped);
            }

            Assert.AreSame(_context.Locator, ambientLocator.Current);
            Assert.IsFalse(ambientLocator.IsScoped);
        }

        [TestMethod]
        public void ShouldHaveScopeAccessInScope()
        {
            using (var scope = _context.Locator.Get<ILocatorScopedFactory>().CreateScope())
            {
                Assert.IsNotNull(_scopeAccessor.Service.CurrentScope);
            }
        }

        [TestMethod]
        public void ShouldImplementIServiceProvider()
        {
            Assert.IsTrue(_context.Locator is IServiceProvider);

            using (var scope = CreateScope())
                Assert.IsTrue(scope is IServiceProvider);
        }

        [TestMethod]
        public void ShouldImportContext()
        {
            Assert.AreEqual(_context, TestSetup.TestContext);
        }

        [TestMethod]
        public void ShouldNotResolveILocatorScopeOutsideofScope()
        {
            bool failed = false;
            var locator = _context.Locator;

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
            var factory = _context.Locator.Get<IServiceScopeFactory>();

            var scope1 = factory.CreateScope();
            Assert.IsNotNull(scope1);
            scope1.Dispose();

            var scope2 = factory.CreateScope();
            Assert.IsNotNull(scope2);
            scope2.Dispose();
        }

        [TestMethod]
        public void ShouldResolveAll()
        {
            var sut = _context.Locator.GetAll(typeof(IStartupModule)).ToList();
            var sut2 = _context.Locator.GetAll<IStartupModule>().ToList();
            Assert.IsTrue(sut.Any());
            Assert.IsTrue(sut2.Any());
            Assert.IsTrue(sut.Count() == sut2.Count());
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyInternalConstructor()
        {
            Assert.IsNotNull(_context.Locator.Get<Mocks.RegistrationTestGreedyInternal>());
        }

        [TestMethod]
        public void ShouldResolveClassWithGreedyPrivateConstructor()
        {
            Assert.IsNotNull(_context.Locator.Get<Mocks.RegistrationTestGreedyPrivate>());
        }

        [TestMethod]
        public void ShouldResolveClassWithStaticConstructor()
        {
            Assert.IsNotNull(_context.Locator.Get<Mocks.RegistrationTestStatic>());
        }

        [TestMethod]
        public void ShouldResolveComplexFuncCreator()
        {
            var injectedArg = new TestInjectable() { Id = 42 };
            var sut = _context.Locator.Get<Func<IInjectable, TestFuncCreationComplex>>();
            var created = sut(injectedArg);

            Assert.IsTrue(created.InjectionTest.Id == 42);
        }

        [TestMethod]
        public void ShouldResolveScopedTypeWithInjectedLocator()
        {
            var locator = _context.Locator;
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
            var sut = _context.Locator.Get<Func<ILocatorScopedFactory>>();
            var sut1 = sut();
            var sut2 = sut();

            Assert.IsNotNull(sut);
            Assert.AreSame(sut1, sut2);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ShouldThrowLocatorWithNoCreateScope()
        {
            var sut = new LocatorScopeFactory(new BadLocator(), null);
            sut.CreateScope();
        }

        [ExpectedException(typeof(MissingImplementationException))]
        [TestMethod]
        public void ShouldThrowLocatorAccessorException()
        {
            var sut = new LocatorScopeFactory(new StubLocatorForScope(), null);
            sut.CreateScope();
        }

        private ILocatorScoped CreateScope(ILocator locator = null) => ((locator ?? _context.Locator) as ILocatorWithCreateScope).CreateScope();

        private class StubLocatorForScope : ILocator, ILocatorWithCreateScope, ILocatorScoped
        {
            public ILocatorScoped Parent => throw new NotImplementedException();

            public ILocatorScoped CreateScope()
            {
                return this;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            private class BadAccessor : ILocatorScopedAccessor
            {
                public ILocatorScoped CurrentScope => throw new NotImplementedException();
            }

            public object Get(Type serviceType, string key = null)
            {
                if(serviceType == typeof(ILocatorScopedAccessor))
                {
                    return new BadAccessor();
                }

                throw new NotImplementedException();
            }

            public T Get<T>(string key = null) => (T)Get(typeof(T), key);

            public IEnumerable<T> GetAll<T>(string key = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> GetAll(Type serviceType, string key = null)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }

            public void OnDispose(Action disposeAction)
            {
                throw new NotImplementedException();
            }
        }

        private class BadLocator : ILocator
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public object Get(Type serviceType, string key = null)
            {
                throw new NotImplementedException();
            }

            public T Get<T>(string key = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> GetAll<T>(string key = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> GetAll(Type serviceType, string key = null)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
    }
}