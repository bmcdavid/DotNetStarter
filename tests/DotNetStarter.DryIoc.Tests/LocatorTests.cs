using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class LocatorTests
    {
        public Import<IStartupContext> _Context { get; set; }

        [TestMethod]
        public void ShouldImportContext()
        {
            Assert.AreEqual(_Context.Service, ApplicationContext.Default);
        }
    }
}