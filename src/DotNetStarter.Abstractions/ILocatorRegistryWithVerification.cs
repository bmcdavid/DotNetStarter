namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides a mechanism to for register to complete registration and perform any verification steps
    /// </summary>
    public interface ILocatorRegistryWithVerification
    {
        /// <summary>
        /// Allows ILocatorRegistry to perform any final tasks after container setup complete is invoked
        /// </summary>
        void Verify();
    }
}