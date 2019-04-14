using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    internal interface IRemove { }

    [Registration(typeof(IRemove), Lifecycle.Transient)]
    internal class Remove : IRemove { }
}