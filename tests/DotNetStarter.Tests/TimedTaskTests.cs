using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class TimedTaskTests
    {        
        [TestMethod]
        public void ShouldHaveTimers()
        {
            var timers = Context.Default.Configuration.TimedTaskManager.GetAll(nameof(DotNetStarter));

            Assert.IsTrue(timers.Any());
        }        
    }
}
