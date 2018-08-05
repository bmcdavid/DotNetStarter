using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Web.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var scannableAssemblies = new List<Assembly>
            {
                typeof(DotNetStarter.ApplicationContext).Assembly,
                typeof(DotNetStarter.Abstractions.IAssemblyFilter).Assembly,
                typeof(DotNetStarter.Locators.DryIocLocatorRegistry).Assembly,
                typeof(DotNetStarter.Web.IHttpContextProvider).Assembly,
                typeof(Mocks.MockHttpContextProvider).Assembly,
            };

            Configure.StartupBuilder.Create()
                .ConfigureAssemblies(a => a.WithAssemblies(scannableAssemblies))
                .Run();
        }
    }
}