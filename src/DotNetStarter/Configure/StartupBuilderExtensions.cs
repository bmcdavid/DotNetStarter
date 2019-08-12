namespace DotNetStarter.Configure
{
    /// <summary>
    /// Allows for a static startup context
    /// </summary>
    public static class StartupBuilderExtensions
    {
        internal static StartupBuilder StaticBulder;

        /// <summary>
        /// Ensures same instance is always used throughout application lifetime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static StartupBuilder AsStatic(this StartupBuilder s)
        {
            if (StaticBulder is null)
            {
                StaticBulder = s;
            }

            return StaticBulder;
        }
    }
}