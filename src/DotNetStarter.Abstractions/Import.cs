namespace DotNetStarter.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to locator services
    /// <para>Important: Import&lt;T> should only be used when constructor injection is not an option;
    /// when used it should be public to disclose the dependency to consumers.</para>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    [CriticalComponent]
    public struct Import<TService> where TService : class
    {
        /// <summary>
        /// The accessor allows for services to be set with a locator.
        /// </summary>
        public ImportAccessor<TService> Accessor { get; set; }

        /// <summary>
        /// Allows ambient imports to be disabled
        /// </summary>
        public static bool DisableAmbientLocator { get; set; }

        /// <summary>
        /// Access to a single service
        /// </summary>
        public TService Service
        {
            get
            {
                if (Accessor != null) { return Accessor.Service; }

                return ResolveLocator().Get<TService>();
            }
        }

        /// <summary>
        /// Access to all services registered
        /// </summary>
        public IEnumerable<TService> AllServices
        {
            get
            {
                if (Accessor != null) { return Accessor.AllServices; }

                return ResolveLocator().GetAll<TService>();
            }
        }

        private ILocator ResolveLocator() => (!DisableAmbientLocator) ?
            ImportHelper.Locator.Get<ILocatorAmbient>().Current :
            ImportHelper.Locator;
    }
}