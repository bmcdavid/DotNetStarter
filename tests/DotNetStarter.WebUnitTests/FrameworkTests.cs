using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace DotNetStarter.Web.Tests
{
    [TestClass]
    public class FrameworkTests
    {
        Import<IHttpContextProvider> _HttpContextProvider;
        Import<IWebModuleStartupHandler> _WebModuleHandler;

        [TestMethod]
        public void ShouldCreateStartupEnvironmentWeb()
        {
            var x = new StartupConfigurationWithWebEnvironment(
                System.AppDomain.CurrentDomain.GetAssemblies(),
                new StartupEnvironmentWeb("UnitTest"),
                new AssemblyFilter(),
                new AssemblyScanner(),
                new DependencyFinder(),
                new DependencySorter(new StartupObjectFactory().CreateDependencyNode),
                new StringLogger(LogLevel.Error, 10000),
                new StartupModuleFilter(),
                new TimedTaskManager(new StartupObjectFactory().CreateRequestSettingsProvider)
            );

            Assert.AreSame(x.Environment.EnvironmentName, "UnitTest");

            Assert.IsNotNull(x.Assemblies);
            Assert.IsNotNull(x.AssemblyFilter);
            Assert.IsNotNull(x.AssemblyScanner);
            Assert.IsNotNull(x.DependencyFinder);
            Assert.IsNotNull(x.DependencySorter);
            Assert.IsNotNull(x.Logger);
            Assert.IsNotNull(x.ModuleFilter);
            Assert.IsNotNull(x.TimedTaskManager);
        }

        [TestMethod]
        public void ShouldDefaultToRegisterHttpContextBase()
        {
            Assert.IsTrue(DotNetStarter.Web.WebConfiguration.RegisterScopedHttpContext);
        }

        [TestMethod]
        public void ShouldLocateHttpProviderContext()
        {
            var sut = _HttpContextProvider.Service.CurrentContext;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Request.Url.PathAndQuery == "/file?a=b&c=d");
        }

        [TestMethod]
        public void ShouldInitMockModuleWithWebModuleHandler()
        {
            _WebModuleHandler.Service.Startup(new System.Web.HttpApplication(), new IHttpModule[] { new Mocks.MockHttpModule() });

            Assert.IsTrue(_WebModuleHandler.Service.StartupEnabled());
            Assert.IsTrue(Mocks.MockHttpModule.InitCalled);
        }

        [TestMethod]
        public void ShouldNotInitMockModuleWithWebModuleHandler()
        {
            Mocks.MockHttpModule.InitCalled = false; //reset
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var handler = new Mocks.DisabledWebModuleHandler(locator);
            
            if(handler.StartupEnabled())
                handler.Startup(new System.Web.HttpApplication(), new IHttpModule[] { new Mocks.MockHttpModule() });

            Assert.IsFalse(handler.StartupEnabled());
            Assert.IsFalse(Mocks.MockHttpModule.InitCalled);
        }

        [TestMethod]
        public void ShouldStartupModuleWithEmptyConstructor()
        {
            Mocks.MockHttpModule.InitCalled = false; //reset
            var appStartup = new DotNetStarter.Web.WebModuleStartup();

            appStartup.Init(new System.Web.HttpApplication());            
        }
    }
}