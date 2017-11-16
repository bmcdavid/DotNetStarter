using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DotNetStarter.Abstractions;
using DotNetStarter.Tests.Mocks;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class ImportTest
    {
        public Import<IFoo> Foo { get; set; }

        public Import<ILocator> Locator;

        public Import<ITransient> Transient;

        [TestMethod]
        public void ShouldImportDifferentInstances()
        {
            Assert.AreNotEqual(Transient.Service, Transient.Service);
        }

        [TestMethod]
        public void ShouldImportAllServices()
        {
            Assert.IsTrue(Foo.AllServices.Count() > 1);
        }

        [TestMethod]
        public void ShouldImportInOrder()
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
    }
}