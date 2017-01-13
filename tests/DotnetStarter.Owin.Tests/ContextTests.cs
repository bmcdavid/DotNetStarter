using DotNetStarter.Owin.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Owin.Tests
{
    [TestClass]
    public class ContextTests
    {
        IMiddlewareContext _MiddlewareContext;

        [TestInitialize]
        public void Setup()
        {
            _MiddlewareContext = new MiddlewareContext(GetMockEnvironment());
        }

        [TestMethod]
        public void ShouldCreateEmptyContext()
        {
            Assert.IsNotNull(new MiddlewareContext());
        }

        [TestMethod]
        public void ShouldCreateContextFromEnvironment()
        {
            Assert.IsNotNull(new MiddlewareContext(GetMockEnvironment()));
        }

        [TestMethod]
        public void ShouldGetRequestUrl()
        {
            Assert.IsTrue(_MiddlewareContext.Request.QueryString.Contains("a=b"));
        }

        [TestMethod]
        public void ShouldGetServerPort()
        {
            Assert.IsTrue(_MiddlewareContext.Request.Uri.Port == 27417);
        }

        [TestMethod]
        public void ShouldGetRequestUri()
        {
            var sut = _MiddlewareContext.Request.Uri;

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void ShouldGetSecureRequest()
        {
            Assert.IsTrue(_MiddlewareContext.Request.IsSecure);
        }

        [TestMethod]
        public void ShouldSetRedirect()
        {
            _MiddlewareContext.Response.Redirect("/contactus");

            Assert.IsTrue(_MiddlewareContext.Response.StatusCode == 302);
            Assert.IsTrue(_MiddlewareContext.Response.Headers.ContainsKey("Location"));
        }

        [TestMethod]
        public void ShouldGetContextItem()
        {
            Assert.IsTrue(_MiddlewareContext.Get<IDictionary<string, string[]>>(MiddlewareOwinConstants.RequestHeaders) is IDictionary<string, string[]>);
        }

        [TestMethod]
        public void ShouldSetContextItem()
        {
            _MiddlewareContext.Set<int>(nameof(ContextTests), 100);

            Assert.AreEqual<int>(100, _MiddlewareContext.Get<int>(nameof(ContextTests)));
        }

        private IDictionary<string, object> GetMockEnvironment()
        {
            Dictionary<string, object> owinEnvironment = new Dictionary<string, object>()
            {
                { MiddlewareOwinConstants.OwinVersion, "1.0" },
                { MiddlewareOwinConstants.CallCancelled, System.Threading.CancellationToken.None },
                { MiddlewareOwinConstants.RequestProtocol, "HTTP/1.1" },
                { MiddlewareOwinConstants.RequestScheme, "https" },
                { MiddlewareOwinConstants.RequestMethod, "GET" },
                { MiddlewareOwinConstants.RequestPathBase, "" },
                { MiddlewareOwinConstants.RequestPath, "/aboutus" },
                { MiddlewareOwinConstants.RequestQueryString, "a=b&c=d" },
                { MiddlewareOwinConstants.RequestHeaders, new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase) },
                { MiddlewareOwinConstants.RequestBody, null}, //Microsoft.AspNetCore.Server.Kestrel.Internal.Http.FrameRequestStream
                { "owin.RequestUser", null },

                { "server.User", null },
                { "server.ConnectionId", "0HKV72R0U3SDL" },
                { MiddlewareOwinConstants.CommonKeys.LocalIpAddress, "127.0.0.1" },
                { MiddlewareOwinConstants.CommonKeys.LocalPort, "27417" },
                { MiddlewareOwinConstants.CommonKeys.RemoteIpAddress, "::1" },
                { MiddlewareOwinConstants.CommonKeys.RemotePort,"56374" },
                { MiddlewareOwinConstants.CommonKeys.OnSendingHeaders, null }, //System.Action`2[System.Action`1[System.Object], System.Object]

                { MiddlewareOwinConstants.ResponseReasonPhrase, "" },
                { MiddlewareOwinConstants.ResponseStatusCode, 200 },
                { MiddlewareOwinConstants.ResponseHeaders, new Dictionary<string,string[]>(StringComparer.OrdinalIgnoreCase) },
                { MiddlewareOwinConstants.ResponseBody, null}, //Microsoft.VisualStudio.Web.BrowserLink.Runtime.ScriptInjectionFilterStream]

            };

            return owinEnvironment;
        }
    }
}