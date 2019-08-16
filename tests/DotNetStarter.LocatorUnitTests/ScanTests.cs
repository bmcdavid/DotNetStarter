using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class ScanTests
    {
        private IAssemblyScanner _assemblyScanner;

        private IReflectionHelper _reflectionHelper;

        [TestInitialize]
        public void Setup()
        {
            _assemblyScanner = TestSetup.TestContext.Configuration.AssemblyScanner;
            _reflectionHelper = TestSetup.TestContext.Locator.Get<IReflectionHelper>();
        }

        [TestMethod]
        public void ShouldScanServiceAttributes()
        {
            var types = _assemblyScanner.GetTypesFor(typeof(RegistrationAttribute));

            Assert.IsTrue(types?.Count() > 0);
        }

        [TestMethod]
        public void ShouldScanBaseClass()
        {
            var types = _assemblyScanner.GetTypesFor(typeof(MockBaseClass));

            Assert.IsTrue(types?.Any(x => x == typeof(MockImplClass)) == true);
        }

        [TestMethod]
        public void ShouldScanInterface()
        {
            var types = _assemblyScanner.GetTypesFor(typeof(IMock));

            Assert.IsTrue(types?.Any(x => x == typeof(MockImplClass)) == true);
        }

        [TestMethod]
        public void ShouldScanOpenGenericInterface()
        {
            var types = _assemblyScanner.GetTypesFor(typeof(IGenericeMock<>));

            Assert.IsTrue(types.Where(x => !_reflectionHelper.IsInterface(x)).Count() == 2);
        }

        [TestMethod]
        public void ShouldScanBoundedGenericInterface()
        {
            var types = _assemblyScanner.GetTypesFor(typeof(IGenericeMock<object>));

            Assert.IsTrue(types.Any(x => x == typeof(GenericObject)));
        }
    }
}