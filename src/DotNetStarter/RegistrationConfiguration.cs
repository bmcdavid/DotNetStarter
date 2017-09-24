namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System.Linq;

    /// <summary>
    /// Registers all types that use RegistrationAttribute to the locator.
    /// </summary>
    [StartupModule]
    public class RegistrationConfiguration : ILocatorConfigure
    {
        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
        {
            var configuration = engine.Configuration;
            var serviceType = typeof(RegistrationAttribute);
            var services = configuration.AssemblyScanner.GetTypesFor(serviceType).ToList();

            for (int i = 0; i < services.Count; i++)
            {
                var attrs = services[i].CustomAttribute(serviceType, false).OfType<RegistrationAttribute>();

                if (attrs?.Any() == true)
                {
                    foreach (var attr in attrs)
                    {
                        registry.Add
                        (
                            attr.ServiceType, // service
                            services[i], // implementation
                            lifeTime: (LifeTime)((int)attr.Lifecycle), // converted lifecycle
                            constructorType: ConstructorType.Greediest
                        );
                    }
                }
            }
        }
    }
}