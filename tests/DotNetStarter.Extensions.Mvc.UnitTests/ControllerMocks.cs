using DotNetStarter.Abstractions;
using System.Web.Mvc;

namespace DotNetStarter.Extensions.Mvc.Tests
{
    public class ControllerOne : Controller
    {
        public ControllerOne(IStartupContext startupContext, ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (locatorScopedAccessor.CurrentScope is null)
                throw new System.NullReferenceException();
        }
    }
}