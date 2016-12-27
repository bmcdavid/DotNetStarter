namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Constructor type
    /// </summary>
    public enum ConstructorType
    {
        /// <summary>
        /// Constructor with most parameters
        /// </summary>
        Greediest = 0,
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        Empty = 1,
        /// <summary>
        /// Constructor with resolved parameters, note: not all containers support this.
        /// </summary>
        Resolved = 2
    }
}
