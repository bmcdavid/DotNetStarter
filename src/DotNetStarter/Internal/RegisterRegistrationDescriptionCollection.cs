using DotNetStarter.Abstractions;
using System.Collections.Generic;

namespace DotNetStarter.Internal
{
    //todo: bump all locator package minors, registration minor, and dotnetstarter minor version numbers
    //todo: implement SourceLink in all packages

    /// <summary>
    /// Default Registration description collection
    /// </summary>
    public class RegistrationDescriptionCollection : List<RegistrationDescriptor>, IRegistrationCollection { }

    /// <summary>
    /// Non discoverable IServiceCollection registration
    /// </summary>
    public class RegisterRegistrationDescriptionCollection : ILocatorConfigure
    {
        private readonly IRegistrationCollection _registrationCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceCollection"></param>
        public RegisterRegistrationDescriptionCollection(IRegistrationCollection serviceCollection) => _registrationCollection = serviceCollection;

        void ILocatorConfigure.Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine) =>
            AddServicesToLocator(_registrationCollection, registry);

        internal static void AddServicesToLocator(IRegistrationCollection services, ILocatorRegistry locator)
        {
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];

                if (service.ImplementationType != null)
                {
                    locator.Add(service.ServiceType, service.ImplementationType, lifecycle: service.Lifetime);
                }
                else if (service.ImplementationFactory != null)
                {
                    locator.Add(service.ServiceType,
                    l => service.ImplementationFactory(l.GetServiceProvider()),
                    service.Lifetime);
                }
                else
                {
                    locator.Add(service.ServiceType, service.ImplementationInstance);
                }
            }
        }
    }
}