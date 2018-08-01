namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Allows for registration attribute customization
    /// </summary>
    public interface IRegistrationLifecycleModifier
    {
        /// <summary>
        /// Change the registration lifecycle
        /// </summary>
        /// <param name="registrationAttribute"></param>
        /// <returns></returns>
        Lifecycle? ChangeLifecycle(RegistrationAttribute registrationAttribute);
    }
}