using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Default is to disable DotNetStarter.Web's scope
    /// </summary>
    [Registration(typeof(IPipelineScope), Lifecycle.Singleton)]
    public class PipelineScope : IPipelineScope
    {
        /// <summary>
        /// Default is false
        /// </summary>
        public virtual bool Enabled => false;
    }
}