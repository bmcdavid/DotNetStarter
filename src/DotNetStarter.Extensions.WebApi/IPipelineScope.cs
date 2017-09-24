namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Enables the scoped container opened in DotNetStarter.Web
    /// </summary>
    public interface IPipelineScope
    {
        /// <summary>
        /// If true, the scoped locator used in the HttpContext.Items is used, otherwise, scope is only for WebApi 
        /// </summary>
        bool Enabled { get; }
    }
}
