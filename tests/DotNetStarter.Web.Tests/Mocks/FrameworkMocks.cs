using DotNetStarter.Abstractions;
using System.Collections.Generic;
using System.Web;

namespace DotNetStarter.Web.Tests.Mocks
{
    [Register(typeof(IHttpContextProvider), LifeTime.Singleton, typeof(HttpContextProvider))]
    public class MockHttpContextProvider : IHttpContextProvider
    {
        public HttpContextBase CurrentContext => new HttpContextWrapper(new HttpContext
            (
            new HttpRequest("", "http://localhost/file?a=b&c=d",""),
            new HttpResponse(null)
            ));
    }

    public class MockHttpModule : IHttpModule
    {
        internal static bool InitCalled = false;

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            InitCalled = true;
        }
    }

    public class DisabledWebModuleHandler : DotNetStarter.Web.WebModuleStartupHandler
    {
        public DisabledWebModuleHandler(ILocator locator, IEnumerable<IStartupModule> modules) : base(locator.Get<ILocatorScopedFactory>(), modules)
        {

        }

        public override bool ScopeEnabled()
        {
            return false;
        }

        public override bool StartupEnabled()
        {
            return false;
        }
    }
}