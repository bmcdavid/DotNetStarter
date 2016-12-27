using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Internal;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class AssemblyLoaderTests
    {
        [TestMethod]
        public void ShouldOverrideDefaultLoader()
        {
            Assert.IsTrue(AssemblyLoader.Default is TestAssemblyLoader);
        }
    }
}
