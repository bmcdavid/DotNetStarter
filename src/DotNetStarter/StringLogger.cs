namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Text;

    /// <summary>
    /// Provides a simpler logger that doesn't use disk storage.
    /// </summary>
    public class StringLogger : IStartupLogger
    {
        /// <summary>
        /// String builder to store messages
        /// </summary>
        protected StringBuilder StringBuilderLogger = new StringBuilder(200);

        /// <summary>
        /// Returns stringbuilder contents
        /// </summary>
        /// <returns></returns>
        public override string ToString() => StringBuilderLogger.ToString();

        /// <summary>
        /// Logs exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="source"></param>
        /// <param name="level"></param>
        public void LogException(string message, Exception e, Type source, LogLevel level)
        {
            StringBuilderLogger.AppendLine(string.Format("{0}: {1} from {2} at {3}", level.ToString(), message ?? "no message", source.FullName, DateTime.Now.ToString()));
            
            StringBuilderLogger.AppendLine(string.Format("Exception Details: {0}", (e ?? new Exception()).ToString()));
            StringBuilderLogger.AppendLine("###########" + Environment.NewLine);
        }

        /// <summary>
        /// Converts line breaks to br tags.
        /// </summary>
        public virtual string ToWebString => ToString().Replace(Environment.NewLine, "<br />");
    }
}