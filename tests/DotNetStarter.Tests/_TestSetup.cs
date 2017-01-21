using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Internal;

namespace DotNetStarter.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            AssemblyLoader.SetAssemblyLoader(new TestAssemblyLoader());
            ApplicationContext.Startup();
        }
    }

    public class TestAssemblyLoader : AssemblyLoader
    {
        public override IEnumerable<Assembly> GetAssemblies() => base.GetAssemblies();
    }
}
