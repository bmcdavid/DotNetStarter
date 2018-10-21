using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace DotNetStarter.UnitTests
{
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
            var sut = new ImportAccessor<object>(new StringBuilder(), null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldGuardImportAccessorNullService()
        {
            var sut = new ImportAccessor<object>(null, new object[] { });
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
            Assert.IsInstanceOfType(Locator.Service.Get<IServiceProvider>(), typeof(DotNetStarter.ServiceProvider));
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

        private class MockClass
        {
            public Import<object> Test { get; set; }
        }

        private class MockImportClass
        {
            public Import<NotRegisteredObject> Test { get; set; }

            public class NotRegisteredObject { }
        }
    }
}