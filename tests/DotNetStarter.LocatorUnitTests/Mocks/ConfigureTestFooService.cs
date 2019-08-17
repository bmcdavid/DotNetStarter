using DotNetStarter.Abstractions;
using DotNetStarter.Configure;

namespace DotNetStarter.UnitTests.Mocks
{
    [StartupModule]
    public class ConfigureTestFooService : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            registry.Add<TestFoo, TestFoo>();
        }
    }

    public class BadStaticTest : ILocatorConfigure
    {
        internal static bool Executed = false;
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            Executed = true;
            if(((IStartupContext)null).AsStatic() is object)
            {
                throw new System.Exception("This is bad and should never be used!");
            }
        }
    }
}