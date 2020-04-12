using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ManualLocatorConfigure : ILocatorConfigure
    {
        public bool Executed { get; private set; }

        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine)
        {
            Executed = true;
        }
    }
}