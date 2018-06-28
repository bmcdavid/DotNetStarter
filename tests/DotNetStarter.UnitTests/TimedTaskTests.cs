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
            var timers = ApplicationContext.Default.Configuration.TimedTaskManager.GetAll(nameof(DotNetStarter));

            Assert.IsTrue(timers.Any());
        }        
    }
}
