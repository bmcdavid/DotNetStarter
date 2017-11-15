using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace DotNetStarter.Extensions.WebApi.Tests
{
    [TestClass]
    public class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var scannableAssemblies = ApplicationContext.GetScannableAssemblies();

            ApplicationContext.Startup(assemblies: scannableAssemblies.Union(new Assembly[] { typeof(_TestSetup).Assembly }));
        }
    }
}