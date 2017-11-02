using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using StructureMap;

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Scoped Episerver locator
    /// </summary>
    public sealed class EpiserverLocatorScoped : EpiserverStructuremapLocatorBase, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="scopeKind"></param>
        /// <param name="locator"></param>
        public EpiserverLocatorScoped(IContainer container, IScopeKind scopeKind, ILocator locator) : base(container)
        {
            Parent = locator as ILocatorScoped;
            ScopeKind = scopeKind;

            // Critical component to replace application ILocator with scoped one
            container.Configure(x =>
            {
                x.For(typeof(ILocator)).Singleton().Use(this);
                x.For(typeof(ILocatorScoped)).Singleton().Use(this);
            });
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// ScopeKind request
        /// </summary>
        public IScopeKind ScopeKind { get; }
    }
}