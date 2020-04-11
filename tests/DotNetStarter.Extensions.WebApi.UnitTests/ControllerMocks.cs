using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;

namespace DotNetStarter.Extensions.WebApi.Tests
{
    [ExcludeFromCodeCoverage]
    public class ControllerOne : ApiController
    {
        public ControllerOne(IStartupContext startupContext, ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (locatorScopedAccessor.CurrentScope is null)
                throw new System.NullReferenceException();
        }
    }
}