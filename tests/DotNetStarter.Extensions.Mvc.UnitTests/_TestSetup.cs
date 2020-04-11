using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DotNetStarter.Extensions.Mvc.Tests
{
    [ExcludeFromCodeCoverage]
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
                typeof(DotNetStarter.Extensions.Mvc.StartupMvc).Assembly,
                typeof(TestSetup).Assembly
            };

            DotNetStarter.Configure.StartupBuilder.Create()
                .UseImport()
                .ConfigureAssemblies(a => a.WithAssemblies(scannableAssemblies))
                //.ConfigureAssemblies(a => a.WithDiscoverableAssemblies())
                .OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.DryIocLocatorFactory()))
                .Build()
                .Run();
        }
    }
}