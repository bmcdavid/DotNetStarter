using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Allows for registration customization
    /// </summary>
    public interface IRegistrationsModifier
    {
        /// <summary>
        /// Change the registration lifec
        /// </summary>
        /// <param name="registrations"></param>
        /// <returns></returns>
        void Modify(ICollection<Registration> registrations);
    }
}