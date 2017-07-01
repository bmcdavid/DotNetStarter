﻿namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Adds types to scan for during assembly scan step.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class ScanTypeRegistryAttribute : Attribute
    {
        /// <summary>
        /// Types to scan for.
        /// </summary>
        public Type[] ScanTypes { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanType">Types to scan for implementations of</param>
        public ScanTypeRegistryAttribute(params Type[] scanType)
        {
            ScanTypes = scanType;
        }
    }
}