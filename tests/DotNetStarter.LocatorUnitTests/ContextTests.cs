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
            var sut = ApplicationContext.Default;

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void ShouldOverrideLogger()
        {
            Assert.IsTrue(ApplicationContext.Default.Configuration.Logger is TestLogger);
        }
    }
}