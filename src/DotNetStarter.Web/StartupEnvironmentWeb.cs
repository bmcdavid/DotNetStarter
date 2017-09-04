﻿using DotNetStarter.Abstractions;
using System;

#if NETSTANDARD1_3
using Microsoft.AspNetCore.Hosting;
#else
using System.Configuration;
#endif

namespace DotNetStarter.Web
{
    /// <summary>
    /// Startup Environment for web
    /// </summary>
    public class StartupEnvironmentWeb : StartupEnvironment, IStartupEnvironmentWeb
    {
#if NETSTANDARD1_3
        /// <summary>
        /// Constructor for netstandard
        /// </summary>
        /// <param name="env"></param>
        /// <param name="applicationBasePath"></param>
        public StartupEnvironmentWeb(IHostingEnvironment env, string applicationBasePath = null) : 
            base(env?.EnvironmentName, applicationBasePath ?? AppContext.BaseDirectory)
        {
            ContentRootPath = env?.ContentRootPath;
            WebRootPath = env?.WebRootPath;
        }
#else
        /// <summary>
        /// Constructor for netframework
        /// </summary>
        /// <param name="environmentName">By default, it reads appsetting value for DotNetStarter.Abstractions.IStartupEnvironment.EnvironmentName</param>
        /// <param name="applicationBasePath"></param>
        /// <param name="webRootPath"></param>
        /// <param name="contentRootPath"></param>
        public StartupEnvironmentWeb(string environmentName = null, string applicationBasePath = null, string webRootPath = null, string contentRootPath = null) :
            base
            (
                environmentName ?? ConfigurationManager.AppSettings[$"{typeof(IStartupEnvironment).FullName}.{nameof(IStartupEnvironment.EnvironmentName)}"]?.ToString(),
                applicationBasePath ?? AppDomain.CurrentDomain.BaseDirectory
            )
        {
            var appDomain = AppDomain.CurrentDomain;
            WebRootPath = webRootPath != null ? webRootPath : !string.IsNullOrEmpty(appDomain.SetupInformation.PrivateBinPath) ? appDomain.BaseDirectory : null;
            ContentRootPath = contentRootPath ?? WebRootPath;
        }
#endif
        /// <summary>
        /// Full webroot path of environment
        /// </summary>
        public virtual string WebRootPath { get; }

        /// <summary>
        /// Full content root path of environment
        /// </summary>
        public virtual string ContentRootPath { get; }
    }
}
