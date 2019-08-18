using System;
namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Exception when concrete type doesn't include a required interface.
    /// </summary>
    public class MissingImplementationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="concrete"></param>
        /// <param name="missing"></param>
        public MissingImplementationException(Type concrete, Type missing) :
            base($"{concrete.FullName} must implement {missing.FullName}!")
        {
        }
    }
}
