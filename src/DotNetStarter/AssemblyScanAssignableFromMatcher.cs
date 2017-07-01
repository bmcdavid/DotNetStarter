﻿namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;

    /// <summary>
    /// Matches if registered type is assignable from scanned type
    /// </summary>
    public class AssemblyScanAssignableFromMatcher : IAssemblyScanTypeMatcher
    {
        Type _AttrType = typeof(Attribute);

        /// <summary>
        /// Matches if registered type is assignable from scanned type
        /// </summary>
        public bool IsMatch(Type registeredType, Type scannedType) => !_AttrType.IsAssignableFromCheck(registeredType) &&  registeredType.IsAssignableFromCheck(scannedType);
    }
}