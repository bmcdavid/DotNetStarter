using DotNetStarter.Abstractions;
using System.Web.Http;

namespace DotNetStarter.Extensions.WebApi.Tests
{
    public class ControllerOne : ApiController
    {
        public ControllerOne(IStartupContext startupContext, ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (locatorScopedAccessor.CurrentScope is null)
                throw new System.NullReferenceException();
        }
    }
}