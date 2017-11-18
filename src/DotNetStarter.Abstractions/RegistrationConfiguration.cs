namespace DotNetStarter.Abstractions
{
    using Internal;
    using System;
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
            var services = configuration.AssemblyScanner.GetTypesFor(serviceType);
            var servicesSorted = configuration.DependencySorter.Sort<RegistrationAttribute>(services.OfType<object>());

            for (int i = 0; i < servicesSorted.Count; i++)
            {
                var t = servicesSorted[i].Node as Type;
                var attrs = t.CustomAttribute(serviceType, false).OfType<RegistrationAttribute>().ToList();

                if (attrs.Count > 0)
                {
                    for (int j = 0; j < attrs.Count; j++)
                    {
                        registry.Add
                        (
                            attrs[j].ServiceType, // service
                            t, // implementation
                            lifecycle: attrs[j].Lifecycle
                        );
                    }
                }
            }
        }
    }
}