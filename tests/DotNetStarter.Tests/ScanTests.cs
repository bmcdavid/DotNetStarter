using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DotNetStarter.Abstractions;
using DotNetStarter.Tests;

// this is left until removed to ensure startup handler supports both
[assembly: ScanTypeRegistry(typeof(MockBaseClass))]

// todo: v2, move typeof(MockBaseClass)
[assembly: DiscoverTypes(typeof(IMock), typeof(IGenericeMock<>), typeof(IGenericeMock<object>))]

namespace DotNetStarter.Tests
{
    public interface IGenericeMock<T> where T : new() { }

    public class GenericStringBuilder : IGenericeMock<System.Text.StringBuilder> { }

    public class GenericObject : IGenericeMock<object> { }

    public interface IMock { }

    public abstract class MockBaseClass : IMock { }

    public class MockImplClass : MockBaseClass { }

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
            var types = _AssemblyScanner.GetTypesFor(typeof(RegisterAttribute));

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
