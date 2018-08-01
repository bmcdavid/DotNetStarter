namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// ItemCollection extensions
    /// </summary>
    public static class ItemCollectionExtensions
    {
        /// <summary>
        /// Gets the given item from the collection
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TValue Get<TValue>(this IItemCollection items) => (TValue)items[typeof(TValue)];

        /// <summary>
        /// Sets the item in the collection, passing a null instance removes the items
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="instance"></param>
        public static void Set<TValue>(this IItemCollection items, TValue instance) => items[typeof(TValue)] = instance;
    }
}