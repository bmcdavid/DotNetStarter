using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class ImportTests
    {
        private class MockClass
        {
            public Import<object> Test { get; set; }
        }

        [TestMethod]
        public void ShouldNotSetImportService()
        {
            Assert.IsNull(new MockClass().Test.Service);
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

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldGuardImportAccessorNullService()
        {
            var sut = new ImportAccessor<object>(null, new object[] { });
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldGuardImportAccessorNullForAllServices()
        {
            var sut = new ImportAccessor<object>(new StringBuilder(), null);
        }
    }
}