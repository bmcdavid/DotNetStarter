using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Registers all types that use RegistrationAttribute to the locator.
    /// </summary>
    [StartupModule]
    public class RegistrationConfiguration : ILocatorConfigure
    {
        private static readonly Type RegistrationType = typeof(RegistrationAttribute);

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngineConfigurationArgs args)
        {
            var scannedRegistrations = args.Configuration.AssemblyScanner.GetTypesFor(RegistrationType);
            var registrations = args.Configuration.DependencySorter
                .Sort<RegistrationAttribute>(scannedRegistrations.OfType<object>())
                .SelectMany(ConvertNodeToRegistration)
                .ToList();

            args.Configuration.RegistrationsModifier?.Modify(registrations);
            registrations.All(r => AddRegistration(r, registry));

            //todo: determine if adding the registrations to the environment are useful
            args.Configuration.Environment.Items.Set<ICollection<Registration>>(registrations);
        }

        private bool AddRegistration(Registration registration, ILocatorRegistry registry)
        {
            registry.Add
            (
                registration.ServiceType, // service
                registration.ImplementationType, // implementation
                lifecycle: registration.Lifecycle
            );

            return true;
        }

        private IEnumerable<Registration> ConvertNodeToRegistration(IDependencyNode node)
        {
            var implementationType = node.Node as Type;
            var attrs = implementationType.CustomAttribute(RegistrationType, false).OfType<RegistrationAttribute>();

            return attrs.Select(r => new Registration(r, implementationType));
        }
    }
}