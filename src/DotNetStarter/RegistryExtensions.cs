using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Registry extensions for application developers
    /// </summary>
    public static class RegistryExtensions
    {
        /// <summary>
        /// Returns typed container, only use when container type is known
        /// </summary>
        /// <typeparam name="TContainer"></typeparam>
        /// <param name="registry"></param>
        /// <returns></returns>
        public static TContainer GetContainer<TContainer>(this ILocatorRegistry registry) => (TContainer)registry.InternalContainer;
    }
}
