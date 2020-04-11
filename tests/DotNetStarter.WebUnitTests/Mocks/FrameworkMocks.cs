using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace DotNetStarter.Web.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    [Registration(typeof(IHttpContextProvider), Lifecycle.Singleton, typeof(HttpContextProvider))]
    public class MockHttpContextProvider : IHttpContextProvider
    {
        public HttpContextBase CurrentContext =>
            new HttpContextWrapper
            (
                new HttpContext
                (
                new HttpRequest("", "http://localhost/file?a=b&c=d", ""),
                new HttpResponse(null)
                )
            );
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    public class DisabledWebModuleHandler : DotNetStarter.Web.WebModuleStartupHandler
    {
        public DisabledWebModuleHandler(ILocator locator) : base(locator.Get<ILocatorScopedFactory>(), locator)
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