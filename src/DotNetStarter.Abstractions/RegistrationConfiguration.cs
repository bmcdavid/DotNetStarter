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
        private static readonly Type RegistrationType = typeof(RegistrationAttribute);

        void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngineConfigurationArgs engine)
        {
            var configuration = engine.Configuration;
            var services = configuration.AssemblyScanner.GetTypesFor(RegistrationType);
            var servicesSorted = configuration.DependencySorter.Sort<RegistrationAttribute>(services.OfType<object>());

            for (int i = 0; i < servicesSorted.Count; i++)
            {
                var implementationType = servicesSorted[i].Node as Type;
                var attrs = implementationType.CustomAttribute(RegistrationType, false).OfType<RegistrationAttribute>().ToList();

                if (attrs.Count > 0)
                {
                    for (int j = 0; j < attrs.Count; j++)
                    {
                        var attr = attrs[j];
                        var objLifecycle = configuration.RegistrationLifecycleModifier?.ChangeLifecycle(attr) ??
                            attr.Lifecycle;
                        registry.Add
                        (
                            attr.ServiceType, // service
                            implementationType, // implementation
                            lifecycle: objLifecycle
                        );
                    }
                }
            }
        }
    }
}