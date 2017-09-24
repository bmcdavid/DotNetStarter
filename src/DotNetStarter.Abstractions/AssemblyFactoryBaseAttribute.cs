namespace DotNetStarter.Abstractions
{
    using Internal;
    using System;

    //todo: v2, replace base class with StartupAssemblyDependencyBaseAttribute

    /// <summary>
    /// Base attribute for creating assembly factories
    /// </summary>
    public abstract class AssemblyFactoryBaseAttribute : AssemblyDependencyBaseAttribute
    {
        private static Func<Type, bool> _FactoryIsAbstract = TypeExtensions.IsAbstract;
        private static Func<Type, bool> _FactoryIsInterface = TypeExtensions.IsInterface;
        private static Func<Type, Type, bool> _ImplementationTypeIsAssignableFromFactory = TypeExtensions.IsAssignableFromCheck;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factoryType">Class that creates containers</param>
        /// <param name="implRestriction">Type restriction check</param>
        /// <param name="dependencies"></param>
        public AssemblyFactoryBaseAttribute(Type factoryType, Type implRestriction, params Type[] dependencies) : base(dependencies)
        {
            FactoryType = factoryType;

            if (implRestriction != null)
            {
                if (!ImplementationTypeIsAssignableFromFactory(implRestriction, FactoryType))
                    throw new ArgumentException($"{FactoryType.FullName} does not implement {implRestriction.FullName}!");
            }

            if (FactoryIsAbstract(factoryType) || FactoryIsInterface(factoryType))
                throw new NotSupportedException($"{nameof(factoryType)} cannot be abstract or an interface.");
        }

        /// <summary>
        /// Determines if given factory type is an abstract
        /// </summary>
        public static Func<Type, bool> FactoryIsAbstract
        {
            get
            {
                return _FactoryIsAbstract;
            }
            set
            {
                ThrowIfNull(value, nameof(FactoryIsAbstract));
                _FactoryIsAbstract = value;
            }
        }

        /// <summary>
        /// Determines if factory is an interface
        /// </summary>
        public static Func<Type, bool> FactoryIsInterface
        {
            get
            {
                return _FactoryIsInterface;
            }
            set
            {
                ThrowIfNull(value, nameof(FactoryIsInterface));
                _FactoryIsInterface = value;
            }
        }

        /// <summary>
        /// Determines if given factory type is assignable from implementation restriction
        /// </summary>
        public static Func<Type, Type, bool> ImplementationTypeIsAssignableFromFactory
        {
            get
            {
                return _ImplementationTypeIsAssignableFromFactory;
            }
            set
            {
                ThrowIfNull(value, nameof(ImplementationTypeIsAssignableFromFactory));
                _ImplementationTypeIsAssignableFromFactory = value;
            }
        }

        /// <summary>
        /// Factory Type
        /// </summary>
        /// <returns></returns>
        public Type FactoryType { get; }

        private static void ThrowIfNull(object value, string property)
        {
            if (value == null)
                throw new NullReferenceException(property + $" func cannot be null in {typeof(AssemblyDependencyBaseAttribute).FullName}!");
        }
    }
}
