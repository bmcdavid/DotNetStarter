namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Provides simple service registration to IContainer
    /// </summary>
    [Obsolete("RegisterAttribute will be removed in version 2, please use RegistrationAttribute unless depencency support is needed, then wait for version 2 to change.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute : DependencyBaseAttribute
    {
        //todo: v2, remove this constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="lifeTime"></param>
        /// <param name="constructorType"></param>
        /// <param name="dependencies"></param>
        [Obsolete("Please use the constructor without constructor type.", false)]
        public RegisterAttribute
        (
            Type serviceType,
            LifeTime lifeTime,
            ConstructorType constructorType,
            params Type[] dependencies
        ) : base(dependencies)
        {
            ServiceType = serviceType;
            LifeTime = lifeTime;
            ConstructorType = constructorType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="lifeTime"></param>
        /// <param name="dependencies"></param>
        public RegisterAttribute
        (
            Type serviceType,
            LifeTime lifeTime,
            params Type[] dependencies
        ) : base(dependencies)
        {
            ServiceType = serviceType;
            LifeTime = lifeTime;
            ConstructorType = ConstructorType.Greediest;
        }

        /// <summary>
        /// Service constructor type
        /// </summary>
        public ConstructorType ConstructorType { get; }

        /// <summary>
        /// Implementation of service, not used by DotNetStarter
        /// </summary>
        [Obsolete("No longer used in DotNetStarter!")]
        public Type ImplementationType { get; set; }

        /// <summary>
        /// Service lifetime
        /// </summary>
        public LifeTime LifeTime { get; }

        /// <summary>
        /// Type of service to register
        /// </summary>
        public Type ServiceType { get; }
    }
}