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
            List<Assembly> assemblies = new List<Assembly>
            {
                typeof(DotNetStarter.ApplicationContext).Assembly(),
                typeof(DotNetStarter.Abstractions.RegistrationConfiguration).Assembly(),
                typeof(DotNetStarter.UnitTests.Mocks.ExcludeModule).Assembly()
            };

#pragma warning disable CS0618 // Type or member is obsolete
            ApplicationContext.Startup(assemblies: assemblies, objectFactory: new Mocks.TestObjectFactory());
        }
    }
}