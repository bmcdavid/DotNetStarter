namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections;

    /// <summary>
    /// Default request settings provider
    /// </summary>
    public class RequestSettingsProvider : IRequestSettingsProvider
    {
        /// <summary>
        /// Throws not implemented exception
        /// </summary>
        public virtual IDictionary Items
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns false
        /// </summary>
        public virtual bool IsDebugMode => false;
    }
}