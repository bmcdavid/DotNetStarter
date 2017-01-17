using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DotNetStarter.Abstractions;
using DotNetStarter.Tests;

[assembly: ScanTypeRegistry(typeof(MockBaseClass))]

namespace DotNetStarter.Tests
{
    public abstract class MockBaseClass { }

    public class MockImplClass : MockBaseClass { }

    [TestClass]
    public class ScanTests
    {
        private IAssemblyScanner AssemblyScanner;

        [TestInitialize]
        public void Setup()
        {
            AssemblyScanner = Context.Default.Configuration.AssemblyScanner;
        }

        [TestMethod]
        public void ShouldScanServiceAttributes()
        {
            var types = AssemblyScanner.GetTypesFor(typeof(RegisterAttribute));

            Assert.IsTrue(types?.Count() > 0);
        }

        [TestMethod]
        public void ShouldScanBaseClass()
        {
            var types = AssemblyScanner.GetTypesFor(typeof(MockBaseClass));

            Assert.IsTrue(types?.Any(x => x == typeof(MockImplClass)) == true);
        }
    }
}
