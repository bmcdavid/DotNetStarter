using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Scoped Episerver locator
    /// </summary>
    public class EpiserverLocatorScoped : EpiserverStructuremapLocatorBase, ILocatorScoped
    {
        static readonly IEnumerable<Type> LocatorTypes = new Type[] { typeof(ILocator), typeof(ILocatorScoped) };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="scopeKind"></param>
        public EpiserverLocatorScoped(IContainer container, IScopeKind scopeKind) : base(container)
        {
            IsActiveScope = true;
            ScopeKind = scopeKind;
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
        /// Should always be true in scope
        /// </summary>
        public bool IsActiveScope { get; }

        /// <summary>
        /// ScopeKind request
        /// </summary>
        public IScopeKind ScopeKind { get; }
    }
}