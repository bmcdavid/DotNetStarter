namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Adds ability to build up objects with injectable properties
    /// </summary>
    public interface ILocatorWithPropertyInjection
    {
        /// <summary>
        /// Build up object from container
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool BuildUp(object target);
    }
}