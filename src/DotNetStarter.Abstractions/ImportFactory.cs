namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Creates new T instances per call
    /// <para>Important: ImporFactoryt&lt;T> should only be used when constructor injection is not an option;
    /// when used it should be public to disclose the dependency to consumers.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CriticalComponent]
    public struct ImportFactory<T> where T : class
    {
        /// <summary>
        /// Factory Accessor
        /// </summary>
        public ImportFactoryAccessor<T> Accessor { get; set; }

        /// <summary>
        /// Instance creator
        /// </summary>
        /// <returns></returns>
        public T CreateInstance()
        {
            if (Accessor is object) { return Accessor.Factory(); }

            return ResolveLocator().Get<T>();
        }

        /// <summary>
        /// Allows ambient imports to be disabled
        /// </summary>
        public static bool DisableAmbientLocator { get; set; }

        private ILocator ResolveLocator() => (!DisableAmbientLocator) ?
            ImportHelper.Locator.Get<ILocatorAmbient>().Current :
            ImportHelper.Locator;
    }
}