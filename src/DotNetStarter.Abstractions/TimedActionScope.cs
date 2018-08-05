namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Scope of timed action
    /// </summary>
    public enum TimedActionScope
    {
        /// <summary>
        /// Stores in application variable
        /// </summary>
        Application = 0,

        /// <summary>
        /// Stores in request variable
        /// </summary>
        Request = 100
    }
}