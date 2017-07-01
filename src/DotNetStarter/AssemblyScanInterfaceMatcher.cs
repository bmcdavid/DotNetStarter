namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using System;

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