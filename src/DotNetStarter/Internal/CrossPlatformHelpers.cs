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
#if NET35 || NET40 || NET45
            return string.Compare(a, b, true) == 0;
#else
            return string.Compare(a,b, System.StringComparison.CurrentCultureIgnoreCase) == 0;
#endif
        }

        /// <summary>
        /// For netframework gets the base directory, for netstandard returns empty string
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationBaseDirectory()
        {
#if NET35 || NET40 || NET45
            return System.AppDomain.CurrentDomain.BaseDirectory;
#else
            return "";
#endif
        }
    }
}
