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
    }
}
