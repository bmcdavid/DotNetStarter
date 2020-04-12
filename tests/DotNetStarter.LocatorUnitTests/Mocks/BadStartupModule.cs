using DotNetStarter.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class BadStartupModule : IStartupModule
    {
        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Startup(IStartupEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}