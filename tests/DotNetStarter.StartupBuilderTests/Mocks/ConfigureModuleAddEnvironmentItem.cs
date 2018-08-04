using DotNetStarter.Abstractions;
using System.Text;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    public class ConfigureModuleAddEnvironmentItem : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine configArgs)
        {
            configArgs.Configuration.Environment.Items.Set<StringBuilder>(new StringBuilder());
            configArgs.Configuration.Environment.Items.Get<StringBuilder>().AppendLine("Configured Item!");
        }
    }

    public class StartupModuleUseEnvironmentItem : IStartupModule
    {
        public void Shutdown()
        {
        }

        public void Startup(IStartupEngine engine)
        {
            engine.Configuration.Environment.Items.Get<StringBuilder>().AppendLine("Started Item!");
        }
    }
}