using System.Collections.Generic;
using System.Linq;
using DotNetStarter.Abstractions;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    public class MockRegistrationModifier : IRegistrationsModifier
    {
        public void Modify(ICollection<Registration> registrations)
        {
            registrations.Where(r => r.ServiceType == typeof(TestFooImport)).All(r =>
            {
                r.Lifecycle = Lifecycle.Singleton;
                return true;
            });
        }
    }
}