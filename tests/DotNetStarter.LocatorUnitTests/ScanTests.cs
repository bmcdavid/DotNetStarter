using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class ScanTests
    {
        private IAssemblyScanner _AssemblyScanner;

        private IReflectionHelper _ReflectionHelper;

        [TestInitialize]
        public void Setup()
        {
            _AssemblyScanner = ApplicationContext.Default.Configuration.AssemblyScanner;
            _ReflectionHelper = ApplicationContext.Default.Locator.Get<IReflectionHelper>();
        }

        [TestMethod]
        public void ShouldScanServiceAttributes()
        {
            var types = _AssemblyScanner.GetTypesFor(typeof(RegistrationAttribute));

            Assert.IsTrue(types?.Count() > 0);
        }

        [TestMethod]
        public void ShouldScanBaseClass()
        {
            var types = _AssemblyScanner.GetTypesFor(typeof(MockBaseClass));

            Assert.IsTrue(types?.Any(x => x == typeof(MockImplClass)) == true);
        }

        [TestMethod]
        public void ShouldScanInterface()
        {
            var types = _AssemblyScanner.GetTypesFor(typeof(IMock));

            Assert.IsTrue(types?.Any(x => x == typeof(MockImplClass)) == true);
        }

        [TestMethod]
        public void ShouldScanOpenGenericInterface()
        {
            var types = _AssemblyScanner.GetTypesFor(typeof(IGenericeMock<>));

            Assert.IsTrue(types.Where(x => !_ReflectionHelper.IsInterface(x)).Count() == 2);
        }

        [TestMethod]
        public void ShouldScanBoundedGenericInterface()
        {
            var types = _AssemblyScanner.GetTypesFor(typeof(IGenericeMock<object>));

            Assert.IsTrue(types.Any(x => x == typeof(GenericObject)));
        }
    }
}