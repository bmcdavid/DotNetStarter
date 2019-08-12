using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class TimedTaskTests
    {
        [TestMethod]
        public void ShouldHaveTimers()
        {
            var timers = _TestSetup.TestContext.Configuration.TimedTaskManager.GetAll(nameof(DotNetStarter));

            Assert.IsTrue(timers.Any());
        }
    }
}