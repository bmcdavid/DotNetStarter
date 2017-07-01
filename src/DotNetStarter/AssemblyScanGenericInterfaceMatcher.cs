namespace DotNetStarter
{
    using Abstractions;
    using Abstractions.Internal;
    using System;
    using System.Linq;

    /// <summary>
    /// Matches if registered type is a generic interface
    /// </summary>
    public class AssemblyScanGenericInterfaceMatcher : IAssemblyScanTypeMatcher
    {
        /// <summary>
        /// Matches if registered type is a generic interface
        /// </summary>
        public bool IsMatch(Type registeredType, Type scannedType)
        {
            if (registeredType.IsInterface() && registeredType.IsGenericType())
            {
                return TypeExtensions.
                    Interfaces(scannedType).
                    Where(x => x.IsGenericType()).
                    Any(y => y.GetGenericTypeDefinition() == registeredType);
            }

            return false;
        }
    }
}