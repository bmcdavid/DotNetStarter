using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Default controller registration setup
    /// </summary>
    public class ControllerRegistrationSetup : IControllerRegistrationSetup
    {
        /// <summary>
        /// Default lifecycle is Transient
        /// </summary>
        public virtual LifeTime ControllerLifeTime => LifeTime.Transient;

        /// <summary>
        /// Default is to register controllers
        /// </summary>
        public virtual bool EnableControllerRegisterations => true;
    }
}