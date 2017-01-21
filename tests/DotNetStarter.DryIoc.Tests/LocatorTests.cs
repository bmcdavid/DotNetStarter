using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class LocatorTests
    {
        private Import<IStartupContext> _Context;

        [TestMethod]
        public void ShouldImportContext()
        {
            Assert.AreEqual(_Context.Service, ApplicationContext.Default);
        }
    }
}