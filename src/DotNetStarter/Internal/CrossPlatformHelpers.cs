namespace DotNetStarter.Internal
{
    /// <summary>
    /// Tools used to simplify multiple framework development
    /// </summary>
    public static class CrossPlatformHelpers
    {
        /// <summary>
        /// Compares strings ignoring case
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool StringCompareIgnoreCase(string a, string b)
        {
            return string.Compare(a,b, System.StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// For netframework gets the base directory, for netstandard returns empty string
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationBaseDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}