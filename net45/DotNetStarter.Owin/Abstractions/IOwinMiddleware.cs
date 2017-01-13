namespace DotNetStarter.Owin.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines middleware module
    /// </summary>
    public interface IOwinMiddleware
    {
        /// <summary>
        /// Next item in pipeline
        /// </summary>
        Func<IDictionary<string, object>, Task> Next { get; }

        /// <summary>
        /// Executes module in pipeline
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        Task Invoke(IDictionary<string, object> environment);
    }
}
