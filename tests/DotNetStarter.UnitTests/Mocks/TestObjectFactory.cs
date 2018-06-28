using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    internal class TestObjectFactory : StartupObjectFactory
    {
        public override IStartupLogger CreateStartupLogger() => new TestLogger();

        public override IStartupModuleFilter CreateModuleFilter() => new TestModuleFilter();

        public override ILocatorRegistry CreateRegistry(IStartupConfiguration config) => new TestLocator();

    }
}
