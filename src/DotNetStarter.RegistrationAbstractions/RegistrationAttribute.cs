namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Provides simple object registration for any consumer
    /// </summary>
    /// <remarks>
    /// Important: DotNetStarter will not support dependencies until version 2.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegistrationAttribute : StartupDependencyBaseAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="lifecycle"></param>
        /// <param name="dependencies"></param>
        public RegistrationAttribute(Type serviceType, Lifecycle lifecycle = Lifecycle.Transient, params Type[] dependencies)
            : base(dependencies)
        {
            ServiceType = serviceType;
            Lifecycle = lifecycle;
        }

        /// <summary>
        /// Object lifecycle
        /// </summary>
        public Lifecycle Lifecycle { get; }

        /// <summary>
        /// Type of service to register
        /// </summary>
        public Type ServiceType { get; }
    }
}