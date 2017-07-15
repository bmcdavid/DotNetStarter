namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Allows ILocatorRegistry to perform any final tasks before container setup complete is invoked
    /// </summary>
    public interface ILocatorVerification
    {
        /// <summary>
        /// Allows ILocatorRegistry to perform any final tasks before container setup complete is invoked
        /// </summary>
        void Verify();
    }
}
