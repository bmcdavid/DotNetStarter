namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Import Factory Accessor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImportFactoryAccessor<T> where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory"></param>
        public ImportFactoryAccessor(Func<T> factory) => Factory = factory ?? throw new ArgumentNullException(nameof(factory));

        /// <summary>
        /// Instance Creator
        /// </summary>
        public Func<T> Factory { get; }
    }
}