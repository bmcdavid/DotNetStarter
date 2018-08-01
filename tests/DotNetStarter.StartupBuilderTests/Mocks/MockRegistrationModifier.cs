using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    public class MockRegistrationModifier : IRegistrationLifecycleModifier
    {
        public Lifecycle? ChangeLifecycle(RegistrationAttribute registrationAttribute)
        {
            if (registrationAttribute.ServiceType == typeof(TestFooImport))
            {
                return Lifecycle.Singleton;
            }

            return null;
        }
    }
}