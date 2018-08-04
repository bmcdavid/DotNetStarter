using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// For locators that need extra help
    /// </summary>
    public sealed class ContainerRegistration
    {
        /// <summary>
        /// Registering service type
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// Service key
        /// </summary>
        public string ServiceKey { get; set; }

        /// <summary>
        /// Service Implementation
        /// </summary>
        public Type ServiceImplementation { get; set; }

        /// <summary>
        /// Service instance
        /// </summary>
        public object ServiceInstance { get; set; }

        /// <summary>
        /// Service factory delegate
        /// </summary>
        public Func<ILocator, object> ServiceFactory { get; set; }

        /// <summary>
        /// Service LifeTime
        /// </summary>
        public Lifecycle Lifecycle { get; set; }
    }

    /// <summary>
    /// Collection of services collected during registration
    /// </summary>
    public sealed class ContainerRegistrationCollection : Dictionary<Type, List<ContainerRegistration>>
    {
        /// <summary>
        /// Debug information
        /// </summary>
        /// <returns></returns>
        public string DebugInformation()
        {
#if NET35
            return string.Join(",", this.Select(x => x.Key.FullName).ToArray());
#else
            return string.Join(",", this.Select(x => x.Key.FullName));
#endif
        }
    }
}