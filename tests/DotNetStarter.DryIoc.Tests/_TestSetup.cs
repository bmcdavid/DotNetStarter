using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
#if DRYNETSTANDARD
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.Add(typeof(DotNetStarter.DryIocLocator).Assembly);

            ApplicationContext.Startup(assemblies: assemblies);
#elif MAPNETSTANDARD
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.Add(typeof(DotNetStarter.Locators.StructureMapFactory).Assembly);

            ApplicationContext.Startup(assemblies: assemblies);
#else
            ApplicationContext.Startup();

#endif

        }
    }
}
