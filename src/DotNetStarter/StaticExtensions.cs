using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Configure
{
    /// <summary>
    /// Allows for a static startup context
    /// </summary>
    public static class StaticExtensions
    {
        internal static StartupBuilder StaticBuilder;

        /// <summary>
        /// Ensures same instance is always used throughout application lifetime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static StartupBuilder UseStatic(this StartupBuilder s)
        {
            if (StaticBuilder is null)
            {
                StaticBuilder = s;
            }

            return StaticBuilder;
        }

        /// <summary>
        /// Retrieves the IStartupContext created from a StartupBuilder using the UseStatic() extension
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static IStartupContext AsStatic(this IStartupContext _)
        {
            if (StaticBuilder is null)
            {
                throw new Exception($"Please use the {nameof(UseStatic)} extension in the {nameof(StartupBuilder)}!");
            }

            return StaticBuilder.StartupContext;
        }
    }
}