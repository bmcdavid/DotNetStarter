namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
    using DryIoc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Scoped DryIoc locator
    /// </summary>
    public class DryIocLocatorScoped : DryIocLocatorBase, ILocatorScoped
    {
        static readonly IEnumerable<Type> LocatorTypes = new Type[] { typeof(ILocator), typeof(ILocatorScoped) };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public DryIocLocatorScoped(IContainer container) : base(container)
        {
            IsActiveScope = true;
        }

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
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Should always be true in scoped containers
        /// </summary>
        public bool IsActiveScope { get; }
    }
}
