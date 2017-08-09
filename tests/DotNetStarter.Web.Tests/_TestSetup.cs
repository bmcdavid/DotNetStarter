using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
                            typeof(DotNetStarter.ApplicationContext).GetTypeInfo().Assembly,
                            typeof(DotNetStarter.Abstractions.IAssemblyFilter).GetTypeInfo().Assembly,                            
                            typeof(DotNetStarter.DryIocLocator).GetTypeInfo().Assembly,
            };

#if NETCOREAPP1_1
            assemblies = assemblies.Union(new Assembly[] { typeof(DotNetStarter.Web.NetcoreExtensions).GetTypeInfo().Assembly });
#else
            assemblies = assemblies.Union(new Assembly[] 
                {
                    typeof(DotNetStarter.Web.IHttpContextProvider).GetTypeInfo().Assembly,
                    typeof(Mocks.MockHttpContextProvider).GetTypeInfo().Assembly,
                });
#endif
            ApplicationContext.Startup(assemblies: assemblies);
        }
    }
}
