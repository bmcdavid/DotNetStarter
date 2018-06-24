using DotNetStarter.StartupBuilderTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetStarter.StartupBuilderTests
{
    [TestClass]
    public class SetupTests
    {
        internal static Exception TestImport { get; private set; }

        [AssemblyInitialize]
        public static void Init(TestContext testcontext)
        {
            try
            {
                var sut = new TestFooImport().FooImport.Service;
            }
            catch (Exception e)
            {
                TestImport = e;
            }
        }
    }
}