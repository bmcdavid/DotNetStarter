using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DotNetStarter.Extensions.Mvc.Tests
{
    [TestClass]
    public class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var scannableAssemblies = new Assembly[]
            {
                typeof(DotNetStarter.ApplicationContext).Assembly,
                typeof(DotNetStarter.DryIocLocator).Assembly,
                typeof(DotNetStarter.Web.Startup).Assembly,
                typeof(DotNetStarter.Extensions.Mvc.StartupMvc).Assembly,
                typeof(_TestSetup).Assembly
            };

            DotNetStarter.ApplicationContext.Startup(assemblies: scannableAssemblies);
        }
    }
}