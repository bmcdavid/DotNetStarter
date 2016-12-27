using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DotNetStarter.Abstractions;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class ScanTests
    {
        [TestMethod]
        public void ShouldScanServiceAttributes()
        {
            var types = Context.Default.Configuration.AssemblyScanner.GetTypesFor(typeof(RegisterAttribute));

            Assert.IsTrue(types?.Count() > 0);
        }
    }
}
