namespace DotNetStarter.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void ShouldGetContext()
        {
            var sut = Context.Default;

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void ShouldOverrideLogger()
        {
            Assert.IsTrue(Context.Default.Configuration.Logger is TestLogger);
        }
    }
}