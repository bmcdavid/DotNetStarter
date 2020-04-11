using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc.Tests
{
    [ExcludeFromCodeCoverage]
    public class ControllerOne : Controller
    {
        public ControllerOne(IStartupContext startupContext, ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (locatorScopedAccessor.CurrentScope is null)
                throw new System.NullReferenceException();
        }
    }
}