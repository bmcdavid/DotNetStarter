using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc.Tests
{
    [TestClass]
    public class MvcTests
    {
        private Import<IStartupConfiguration> StartupConfiguration;
        private Import<ILocatorScopedFactory> ScopeFactory;

        [TestMethod]
        public void ShouldScanControllers()
        {
            var controllers = StartupConfiguration.Service.AssemblyScanner.GetTypesFor(typeof(IController));

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

        [TestMethod]
        public void ShouldBeCustomDependencyResolver()
        {
            Assert.IsTrue(DependencyResolver.Current is ScopedDependencyResolver);
        }
    }
}