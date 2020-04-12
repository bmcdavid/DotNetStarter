using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{

    [ExcludeFromCodeCoverage]
    [StartupModule(typeof(RegistrationConfiguration))]
    public class A : ILocatorConfigure
    {
        internal static bool SupportsServiceRemoval = false;

        internal static bool RegisterException = false;

        public void Configure(ILocatorRegistry container, ILocatorConfigureEngine engine)
        {
            try
            {
                container.Add(typeof(IRemove), typeof(BaseImpl), null, Lifecycle.Singleton);
            }
            catch
            {
                RegisterException = true;
            }

            if (container is ILocatorRegistryWithRemove removable)
            {
                removable.Remove(typeof(IRemove));
                SupportsServiceRemoval = true;
            }

            container.Add<BaseTest, BaseImpl>(lifecycle: Lifecycle.Singleton);
            container.Add(typeof(IFooTwo), locator => FooTwoFactory.CreateFoo(), Lifecycle.Transient);
        }
    }
}