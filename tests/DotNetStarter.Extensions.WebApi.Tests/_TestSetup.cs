using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DotNetStarter.Extensions.WebApi.Tests
{
    [TestClass]
    public class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var scannableAssemblies = new Assembly[]
            {
                typeof(DotNetStarter.Abstractions.IAssemblyFilter).Assembly,
                typeof(DotNetStarter.ApplicationContext).Assembly,
                typeof(DotNetStarter.Locators.DryIocLocator).Assembly,
                typeof(DotNetStarter.Web.Startup).Assembly,
                typeof(DotNetStarter.Extensions.WebApi.StartupWebApiConfigure).Assembly,
                typeof(_TestSetup).Assembly
            };

            DotNetStarter.ApplicationContext.Startup(assemblies: scannableAssemblies);
        }
    }
}