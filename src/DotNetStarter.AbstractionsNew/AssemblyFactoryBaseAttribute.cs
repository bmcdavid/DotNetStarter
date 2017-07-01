namespace DotNetStarter.Abstractions
{
    using Internal;
    using System;

    /// <summary>
    /// Base attribute for creating assembly factories
    /// </summary>
    public abstract class AssemblyFactoryBaseAttribute : AssemblyDependencyBaseAttribute
    {
        /// <summary>
        /// Factory Type
        /// </summary>
        /// <returns></returns>
        public Type FactoryType { get; }

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
                if (!implRestriction.IsAssignableFromCheck(FactoryType))
                    throw new ArgumentException($"{FactoryType.FullName} does not implement {implRestriction.FullName}!");
            }

            if (factoryType.IsAbstract() || factoryType.IsInterface())
                throw new NotSupportedException($"{nameof(factoryType)} cannot be abstract or an interface.");
        }
    }
}
