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

        private readonly LogLevel _LogThreshold;

        /// <summary>
        /// Empty Constructor, defaults to log threshold of Error
        /// </summary>
        public StringLogger() : this(LogLevel.Error) { }

        /// <summary>
        /// Injected constructor
        /// </summary>
        /// <param name="logThreshold"></param>
        public StringLogger(LogLevel logThreshold)
        {
            _LogThreshold = logThreshold;
        }

        /// <summary>
        /// Converts line breaks to br tags.
        /// </summary>
        public virtual string ToWebString => ToString().Replace(Environment.NewLine, "<br />");

        /// <summary>
        /// Logs exception if greater than or equal to logger threshold
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="source"></param>
        /// <param name="level"></param>
        public virtual void LogException(string message, Exception e, Type source, LogLevel level)
        {
            if (level >= _LogThreshold)
            {
                StringBuilderLogger.AppendLine(string.Format("{0}: {1} from {2} at {3}", level.ToString(), message ?? "no message", source.FullName, DateTime.Now.ToString()));

                StringBuilderLogger.AppendLine(string.Format("Exception Details: {0}", (e ?? new Exception()).ToString()));
                StringBuilderLogger.AppendLine("###########" + Environment.NewLine);
            }
        }

        /// <summary>
        /// Returns stringbuilder contents
        /// </summary>
        /// <returns></returns>
        public override string ToString() => StringBuilderLogger.ToString();
    }
}