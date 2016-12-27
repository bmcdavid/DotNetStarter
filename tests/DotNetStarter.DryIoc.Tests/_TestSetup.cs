using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetStarter.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            Context.Startup();
        }
    }
}
