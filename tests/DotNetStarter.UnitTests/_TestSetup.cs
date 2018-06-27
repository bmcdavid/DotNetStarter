using DotNetStarter.Abstractions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            List<Assembly> assemblies = new List<Assembly>();
#if NETCOREAPP1_0
            assemblies.Add(typeof(DotNetStarter.ApplicationContext).Assembly());
            assemblies.Add(typeof(DotNetStarter.Abstractions.RegistrationConfiguration).Assembly());
#else
            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
#endif

            ApplicationContext.Startup(assemblies: assemblies, objectFactory: new Mocks.TestObjectFactory());
        }
    }
}