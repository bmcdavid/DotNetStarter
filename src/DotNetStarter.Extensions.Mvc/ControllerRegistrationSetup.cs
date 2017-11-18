using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Default controller registration setup
    /// </summary>
    public class ControllerRegistrationSetup : IControllerRegistrationSetup
    {
        /// <summary>
        /// Default is to register controllers
        /// </summary>
        public virtual bool EnableControllerRegisterations => true;
    }
}