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

        private readonly int _MaxStringBuilderLength;
        private readonly LogLevel _LogThreshold;

        // todo: remove obsolete constructors

        /// <summary>
        /// Empty Constructor, defaults to log threshold of Error
        /// </summary>
        [Obsolete]
        public StringLogger() : this(LogLevel.Error, 1024000) { }

        /// <summary>
        /// Injected constructor
        /// </summary>
        /// <param name="logThreshold"></param>
        [Obsolete]
        public StringLogger(LogLevel logThreshold) : this(logThreshold, 1024000)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logThreshold"></param>
        /// <param name="maxStringLength"></param>
        public StringLogger(LogLevel logThreshold, int maxStringLength)
        {
            _LogThreshold = logThreshold;
            _MaxStringBuilderLength = maxStringLength;
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
                ClearLogger();
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

        /// <summary>
        /// Keeps stringbuilder from growing unbounded
        /// </summary>
        private void ClearLogger()
        {
            if (StringBuilderLogger.Length > _MaxStringBuilderLength)
            {
#if !NET35
                StringBuilderLogger.Clear();
#else
                StringBuilderLogger.Length = 0;
                StringBuilderLogger.Capacity = 0;
#endif
            }
        }
    }
}