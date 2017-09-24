using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.Mvc
{
    /// <summary>
    /// Allows configuration of MVC controller setup
    /// </summary>
    public interface IControllerRegistrationSetup
    {
        /// <summary>
        /// Enables controller registartions by this packages startup module
        /// </summary>
        bool EnableControllerRegisterations { get; }

        /// <summary>
        /// Determiens the controller lifetime
        /// </summary>
        LifeTime ControllerLifeTime { get; }
    }
}
