// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/AspNetKatana/blob/master/LICENSE.txt for license information.
// Modifications copyright 2016 <Brad McDavid>

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