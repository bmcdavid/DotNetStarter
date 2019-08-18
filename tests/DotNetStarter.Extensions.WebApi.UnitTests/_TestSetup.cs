using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DotNetStarter.Extensions.WebApi.Tests
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext _)
        {
            var scannableAssemblies = new Assembly[]
            {
                typeof(DotNetStarter.Abstractions.IAssemblyFilter).Assembly,
                typeof(DotNetStarter.AssemblyFilter).Assembly,
                typeof(DotNetStarter.Locators.DryIocLocatorRegistry).Assembly,
                typeof(DotNetStarter.Web.Startup).Assembly,
                typeof(DotNetStarter.Extensions.WebApi.StartupWebApiConfigure).Assembly,
                typeof(TestSetup).Assembly
            };

            Configure.StartupBuilder.Create()
                .UseImport()
                .ConfigureAssemblies(a => a.WithAssemblies(scannableAssemblies))
                .Run();
        }
    }
}