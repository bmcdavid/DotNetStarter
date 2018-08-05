using System;
using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Allows an assembly to choose how its types are discovered
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ExportsAttribute : Attribute
    {
        /// <summary>
        /// Exports type
        /// </summary>
        public ExportsType ExportsType { get; }

        /// <summary>
        /// Specific exports
        /// </summary>
        public IEnumerable<Type> Exports { get; }

        /// <summary>
        /// Constructor for All and Exports only
        /// </summary>
        /// <param name="exportsType"></param>
        public ExportsAttribute(ExportsType exportsType)
        {
            ExportsType = exportsType;
            Exports = new Type[] { };
        }

        /// <summary>
        /// Constructor for specific exports
        /// </summary>
        /// <param name="specificExports"></param>
        public ExportsAttribute(params Type[] specificExports)
        {
            ExportsType = ExportsType.Specfic;
            Exports = new HashSet<Type>(specificExports);
        }
    }
}