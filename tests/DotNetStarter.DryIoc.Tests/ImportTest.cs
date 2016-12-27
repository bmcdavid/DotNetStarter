using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DotNetStarter.Abstractions;
using DotNetStarter.Tests.Mocks;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class ImportTest
    {
        private ImportAll<IFoo> AllFoo;

        private Import<IFoo> OneFoo;

        private Import<ILocator> Locator;

        [TestMethod]
        public void ShouldImportAllServices()
        {
            Assert.IsTrue(AllFoo.Services.Count() > 1);
        }

        [TestMethod]
        public void ShouldImportLastService()
        {
            Assert.IsTrue(OneFoo.Service is FooService);
        }

        [TestMethod]
        public void ShouldImportPopulatedLocator()
        {
            Assert.AreEqual(Locator.Service.Get<IFoo>(), OneFoo.Service);
        }
    }
}