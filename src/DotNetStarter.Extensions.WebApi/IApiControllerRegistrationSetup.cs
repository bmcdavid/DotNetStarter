using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Allows customization of controller registration
    /// </summary>
    public interface IApiControllerRegistrationSetup
    {
        /// <summary>
        /// Enables controller registrations
        /// </summary>
        bool EnableApiControllerRegistrations { get; }

        /// <summary>
        /// Determines the lifetime registration for each controller
        /// </summary>
        LifeTime ApiControllerLifeTime { get; }
    }
}
