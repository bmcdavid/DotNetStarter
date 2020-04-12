using DotNetStarter.Abstractions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotNetStarter.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
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