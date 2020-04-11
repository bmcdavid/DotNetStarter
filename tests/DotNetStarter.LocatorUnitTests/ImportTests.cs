using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ImportTests
    {
        public Import<ILocator> Locator;

        public Import<ITransient> Transient;

        public Import<IFoo> Foo { get; set; }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldGuardImportAccessorNullForAllServices()
        {
            new ImportAccessor<object>(new StringBuilder(), null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldGuardImportAccessorNullService()
        {
            new ImportAccessor<object>(null, new object[] { });
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldGuardSetImportFactoryAndCreateNewInstances()
        {
            Func<object> nullFactory = null;

            new TestImportFactory
            {
                ObjectFactory = ImportHelper.New(nullFactory)
            };
        }

        [TestMethod]
        public void ShouldImportAllServices()
        {
            Assert.IsTrue(Foo.AllServices.Count() > 1);
        }

        [TestMethod]
        public void ShouldImportDifferentInstances()
        {
            Assert.AreNotEqual(Transient.Service, Transient.Service);
        }

        [TestMethod]
        public void ShouldImportInOrderAsObjects()
        {
            var allFoo = Locator.Service.GetAll(typeof(IFoo)).ToList();

            Assert.IsTrue(allFoo[0] is FooServiceThree, "Order check 0");
            Assert.IsTrue(allFoo[1] is FooService, "Order check 1");
            Assert.IsTrue(allFoo[2] is FooServiceTwo, "Order check 2");
        }

        [TestMethod]
        public void ShouldImportInOrderAsT()
        {
            var allFoo = Foo.AllServices.ToList();

            Assert.IsTrue(allFoo[0] is FooServiceThree, "Order check 0");
            Assert.IsTrue(allFoo[1] is FooService, "Order check 1");
            Assert.IsTrue(allFoo[2] is FooServiceTwo, "Order check 2");
        }

        [TestMethod]
        public void ShouldImportLastService()
        {
            Assert.IsTrue(Foo.Service is FooServiceTwo, "Last Service check");
        }

        [TestMethod]
        public void ShouldImportPopulatedLocator()
        {
            Assert.AreEqual(Locator.Service.Get<IFoo>(), Foo.Service);
        }

        [TestMethod]
        public void ShouldNotSetImportService()
        {
            try
            {
                var sut = new MockImportClass().Test.Service;
            }
            catch
            {
                Assert.IsTrue(true);
                return;
            }

            // hack: catch doesn't work on Grace,Structuremap,StructuremapSigned
        }

        [TestMethod]
        public void ShouldResolveServiceProvider()
        {
            var sut = Locator.Service.Get<IServiceProvider>();
            Assert.IsInstanceOfType(sut, typeof(DotNetStarter.ServiceProvider), sut.GetType().FullName);
        }

        [TestMethod]
        public void ShouldSetImportFactoryAndCreateNewInstances()
        {
            var testClass = new TestImportFactory
            {
                ObjectFactory = ImportHelper.New<object>(() => new object())
            };

            Assert.AreNotSame(testClass.ObjectFactory.CreateInstance(), testClass.ObjectFactory.CreateInstance());
        }

        [TestMethod]
        public void ShouldSetImportServiceFromImportAccessorAsProperty()
        {
            Assert.IsNotNull(new MockClass().Test);
            Assert.IsNull(new MockClass().Test.Accessor);

            var x = new MockClass();
            var i = new Import<object>
            {
                Accessor = new ImportAccessor<object>(new StringBuilder(), new object[] { })
            };
            x.Test = i;

            Assert.IsNotNull(x.Test.Accessor);
        }

        [TestMethod]
        public void ShouldSetServiceFromImportAccessor()
        {
            var i = new Import<object>
            {
                Accessor = new ImportAccessor<object>(new StringBuilder(), new object[] { })
            };

            Assert.IsNotNull(i.Service);
            Assert.IsTrue(i.Service.GetType() == typeof(StringBuilder));
        }

        private class TestImportFactory
        {
            public ImportFactory<object> ObjectFactory { get; set; }
        }
    }
}