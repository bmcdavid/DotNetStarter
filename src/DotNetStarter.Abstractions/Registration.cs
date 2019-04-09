namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// A container registration
    /// </summary>
    public class Registration
    {
        private readonly RegistrationAttribute _registrationAttribute;
        private Lifecycle? _lifecycle;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registrationAttribute"></param>
        /// <param name="implementationType"></param>
        public Registration(RegistrationAttribute registrationAttribute, Type implementationType)
        {
            _registrationAttribute = registrationAttribute;
            ImplementationType = implementationType;
            ServiceType = registrationAttribute.ServiceType;
        }

        /// <summary>
        /// Service Lifecycle
        /// </summary>
        public Lifecycle Lifecycle
        {
            get
            {
                if (_lifecycle is object) return _lifecycle.Value;

                return _registrationAttribute.Lifecycle;
            }
            set => _lifecycle = value;
        }

        /// <summary>
        /// Service Type
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Service Implementation Type
        /// </summary>
        public Type ImplementationType { get; private set; }
    }
}