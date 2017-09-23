namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Used to toggle experimental features
    /// </summary>
    public interface IStartupFeature
    {
        /// <summary>
        /// Feature Enabled
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Feature Name
        /// </summary>
        string Name { get; }
    }
}
