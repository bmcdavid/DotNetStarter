namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Provides simple service registration to IContainer
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute : DependencyBaseAttribute
    {
        /// <summary>
        /// Type of service to register
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Implementation of service
        /// </summary>
        public Type ImplementationType { get; set; }

        /// <summary>
        /// Service lifetime
        /// </summary>
        public LifeTime LifeTime { get; }

        /// <summary>
        /// Service constructor type
        /// </summary>
        public ConstructorType ConstructorType { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="lifeTime"></param>
        /// <param name="constructorType"></param>
        /// <param name="dependencies"></param>
        public RegisterAttribute
        (
            Type serviceType,
            LifeTime lifeTime = LifeTime.Transient,
            ConstructorType constructorType = ConstructorType.Greediest,
            params Type[] dependencies
        ) : base(dependencies)
        {
            ServiceType = serviceType;
            LifeTime = lifeTime;
            ConstructorType = constructorType;
        }
    }
}