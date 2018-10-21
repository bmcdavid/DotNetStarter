using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.UnitTests.Mocks
{
    public interface ITransient
    {
        string Test();
    }

    /// <summary>
    /// Uses registration instead to ensure both types are working
    /// </summary>
    [Registration(typeof(ITransient), Lifecycle.Transient)]
    public class TransientTest : ITransient
    {
        public string Test()
        {
            throw new NotImplementedException();
        }
    }
}