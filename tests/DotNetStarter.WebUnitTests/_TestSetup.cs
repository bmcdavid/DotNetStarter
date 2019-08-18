﻿using DotNetStarter.Configure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Web.Tests
{
    [TestClass]
    public sealed class TestSetup
    {
        public static StartupBuilder Builder { get; private set; }

        [AssemblyInitialize]
        public static void Setup(TestContext _)
        {
            var scannableAssemblies = new List<Assembly>
            {
                typeof(DotNetStarter.AssemblyFilter).Assembly,
                typeof(DotNetStarter.Abstractions.IAssemblyFilter).Assembly,
                typeof(DotNetStarter.Locators.DryIocLocatorRegistry).Assembly,
                typeof(DotNetStarter.Web.IHttpContextProvider).Assembly,
                typeof(Mocks.MockHttpContextProvider).Assembly,
            };

            Builder = Configure.StartupBuilder.Create();
            Builder
                .UseImport()
                .ConfigureAssemblies(a => a.WithAssemblies(scannableAssemblies))
                .Run();
        }
    }
}