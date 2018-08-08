using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter
{
    /// <summary>
    /// Represents a sorted type with its dependencies and attribute instance.
    /// </summary>
    public class DependencyNode : IDependencyNode
    {
        private readonly Type _attributeType;
        private HashSet<object> _dependencies;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="attributeType"></param>
        public DependencyNode(object nodeType, Type attributeType)
        {
            Node = nodeType;
            _attributeType = attributeType;
        }

        /// <summary>
        /// Dependencies
        /// </summary>
        public virtual HashSet<object> Dependencies => BuildDependencies();

        /// <summary>
        /// Count of Dependencies
        /// </summary>
        public virtual int DependencyCount => Dependencies?.Count ?? 0;

        /// <summary>
        /// Full string name of Node
        /// </summary>
        public virtual string FullName => GetFullName(Node);

        /// <summary>
        ///
        /// </summary>
        public bool IsAssembly => Node is Assembly;

        /// <summary>
        /// Node can be a Type or Assembly
        /// </summary>
        public virtual object Node { get; }

        /// <summary>
        /// Attribute instance for Node
        /// </summary>
        public virtual StartupDependencyBaseAttribute NodeAttribute { get; private set; }

        /// <summary>
        /// String representation of DependencyNode in form of (DepedencyCount) Fullname: string delimited fullname list of dependencies
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"({DependencyCount}) {FullName}: {JoinDependencyNames(Dependencies)}.";

        private static string GetFullName(object c) => (c is Type) ? ((Type)c).FullName : ((Assembly)c).FullName;

        private static string JoinDependencyNames(IEnumerable<object> dependencies) =>
            string.Join(",", dependencies.Select(x => GetFullName(x)).ToArray());

        private HashSet<object> BuildDependencies()
        {
            if(_dependencies != null) { return _dependencies; }
            _dependencies = new HashSet<object>();
            var attributes = Node.CustomAttribute(_attributeType, false).OfType<StartupDependencyBaseAttribute>();

            if (attributes?.Any() == true)
            {
                NodeAttribute = attributes.FirstOrDefault();

                foreach (var attribute in attributes)
                {
                    foreach (Type t in attribute.Dependencies)
                    {
                        if (typeof(StartupAssemblyDependencyBaseAttribute).IsAssignableFromCheck(_attributeType))
                        {
                            _dependencies.Add(t.Assembly());
                        }
                        else
                        {
                            _dependencies.Add(t);
                        }
                    }
                }
            }

            return _dependencies;
        }
    }
}