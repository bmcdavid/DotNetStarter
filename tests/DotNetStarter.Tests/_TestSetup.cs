using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetStarter.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            ApplicationContext.Startup(assemblies: AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
