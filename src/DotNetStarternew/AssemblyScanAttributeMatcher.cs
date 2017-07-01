namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Linq;

    /// <summary>
    /// Matches based on if an attribute and scan type has attribute
    /// </summary>
    public class AssemblyScanAttributeMatcher : IAssemblyScanTypeMatcher
    {
        Type _AttrType = typeof(Attribute);

        /// <summary>
        /// Matches based on if an attribute and scan type has attribute
        /// </summary>
        public bool IsMatch(Type registeredType, Type scannedType) => _AttrType.IsAssignableFromCheck(registeredType) && scannedType.CustomAttribute(registeredType, false).Any();
    }
}