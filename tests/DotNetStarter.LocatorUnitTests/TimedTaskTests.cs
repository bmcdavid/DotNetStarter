using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TimedTaskTests
    {
        [TestMethod]
        public void ShouldHaveTimers()
        {
            var timers = TestSetup.TestContext.Configuration.TimedTaskManager.GetAll(nameof(DotNetStarter));

            Assert.IsTrue(timers.Any());
        }
    }
}