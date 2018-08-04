namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Defines how assembly scanning finds types for scanned assembly, if Assembly attribute doesn't exist, All is assumed.
    /// </summary>
    public enum ExportsType
    {
        /// <summary>
        /// Equals Assembly.GetTypes for assembly scanning
        /// </summary>
        All = 0,

        /// <summary>
        /// Equals Assembly.GetExportedTypes for assembly scanning
        /// </summary>
        ExportsOnly = 1,

        /// <summary>
        /// Manually defined type exports
        /// </summary>
        Specfic = 2
    }
}