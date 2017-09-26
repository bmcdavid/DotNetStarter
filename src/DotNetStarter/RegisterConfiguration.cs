namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;
    using System.Linq;

    /// <summary>
    /// Registers all types that use RegisterAttribute to the locator.
    /// </summary>
    [StartupModule]
    public class RegisterConfiguration : ILocatorConfigure
    {
        /// <summary>
        /// Registers all types that use RegisterAttribute to the locator.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="engine"></param>
        public void Configure(ILocatorRegistry container, IStartupEngine engine)
        {
            var configuration = engine.Configuration;
            var serviceType = typeof(RegisterAttribute);
            var services = configuration.AssemblyScanner.GetTypesFor(serviceType);
            var servicesSorted = configuration.DependencySorter.Sort<RegisterAttribute>(services.OfType<object>()).ToList();

            for (int i = 0; i < servicesSorted.Count; i++)
            {
                var t = servicesSorted[i].Node as Type;
                var attrs = t.CustomAttribute(serviceType, false).OfType<RegisterAttribute>();

                if (attrs?.Any() == true)
                {
                    foreach (var attr in attrs)
                    {
                        container.Add(attr.ServiceType, t, null, attr.LifeTime, attr.ConstructorType);
                    }
                }
            }
        }

        /// <summary>
        /// Shutdown
        /// </summary>
        /// <param name="engine"></param>
        public void Shutdown(IStartupEngine engine) { }

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="engine"></param>
        public void Startup(IStartupEngine engine) { }
    }
}