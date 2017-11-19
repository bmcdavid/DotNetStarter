using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;

namespace DotNetStarter.Web.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            IEnumerable<Assembly> assemblies = new Assembly[]
            {
                typeof(DotNetStarter.ApplicationContext).Assembly,
                typeof(DotNetStarter.Abstractions.IAssemblyFilter).Assembly,
                typeof(DotNetStarter.Locators.DryIocLocator).Assembly,
                typeof(DotNetStarter.Web.IHttpContextProvider).Assembly,
                typeof(Mocks.MockHttpContextProvider).Assembly,
            };

            ApplicationContext.Startup(assemblies: assemblies);
        }
    }
}
