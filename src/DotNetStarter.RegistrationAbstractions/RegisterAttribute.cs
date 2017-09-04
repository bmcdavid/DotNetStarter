namespace DotNetStarter.Abstractions
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides simple service registration to IContainer
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute : DependencyBaseAttribute
    {
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
            LifeTime lifeTime = LifeTime.Transient,
            ConstructorType constructorType = ConstructorType.Greediest,
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
            LifeTime lifeTime = LifeTime.Transient,
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
        [EditorBrowsable(EditorBrowsableState.Never)]
#if !NETSTANDARD1_0 && !NETSTANDARD1_1
        [Browsable(false)]
#endif
        public ConstructorType ConstructorType { get; }

        /// <summary>
        /// Implementation of service
        /// </summary>
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