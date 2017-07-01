namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Linq;

    /// <summary>
    /// Matches if scan type has registered interface
    /// </summary>
    public class AssemblyScanInterfaceMatcher : IAssemblyScanTypeMatcher
    {
        /// <summary>
        /// Matches if scan type has registered interface
        /// </summary>
        public bool IsMatch(Type registeredType, Type scannedType)
        {
            return scannedType.HasInterface(registeredType);
        }
    }
}