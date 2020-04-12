using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    [StartupModule]
    public class StartupModuleCompleteTest : IStartupModule
    {
        internal static bool _InitCompleteCalled = false;
        internal static bool InitCalled { get; private set; } = false;
        internal static bool ShutdownCalled { get; private set; } = false;

        public void Shutdown()
        {
            ShutdownCalled = true;
        }

        public void Startup(IStartupEngine engine)
        {
            InitCalled = true;
            engine.OnStartupComplete += Engine_OnStartupComplete;
        }

        private void Engine_OnStartupComplete()
        {
            _InitCompleteCalled = true;
        }
    }
}