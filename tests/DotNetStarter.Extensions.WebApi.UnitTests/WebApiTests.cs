using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;

namespace DotNetStarter.Extensions.WebApi.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MvcTests
    {
        private Import<IStartupConfiguration> StartupConfiguration;
        private Import<ILocatorScopedFactory> ScopeFactory;

        [TestMethod]
        public void ShouldScanControllers()
        {
            var controllers = StartupConfiguration.Service.AssemblyScanner.GetTypesFor(typeof(ApiController));

            Assert.IsTrue(controllers.Any());
        }

        [TestMethod]
        public void ShouldResolveControllers()
        {
            // mocks open web handler scope
            using (var scope = ScopeFactory.Service.CreateScope())
            {
                Assert.IsNotNull(scope.Get<ControllerOne>());
            }
        }
    }
}