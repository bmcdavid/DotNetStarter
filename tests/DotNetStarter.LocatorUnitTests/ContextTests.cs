namespace DotNetStarter.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void ShouldGetContext()
        {
            var sut = TestSetup.TestContext;

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void ShouldOverrideLogger()
        {
            Assert.IsTrue(TestSetup.TestContext.Configuration.Logger is TestLogger);
        }
    }
}