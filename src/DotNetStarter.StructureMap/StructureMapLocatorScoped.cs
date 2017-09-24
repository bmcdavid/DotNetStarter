using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter
{
    /// <summary>
    /// Scoped structuremap locator
    /// </summary>
    public class StructureMapLocatorScoped : StructureMapLocatorBase, ILocatorScoped
    {
        static readonly IEnumerable<Type> LocatorTypes = new Type[] { typeof(ILocator), typeof(ILocatorScoped) };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public StructureMapLocatorScoped(IContainer container) : base(container)
        {
            IsActiveScope = true;
            //Add(typeof(ILocator), this);
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Looks from request for ILocator and replaces with this scoped instance
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object Get(Type service, string key = null)
        {
            if (LocatorTypes.Any(x => x == service))
            {
                return this;
            }

            return base.Get(service, key);
        }

        /// <summary>
        /// Looks from request for ILocator and replaces with this scoped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public override T Get<T>(string key = null)
        {
            return (T)Get(typeof(T), key);
        }

        /// <summary>
        /// Should always be true in scoped locator
        /// </summary>
        public bool IsActiveScope { get; }
    }
}