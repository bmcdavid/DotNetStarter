// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/HttpAbstractions/blob/master/LICENSE.txt for license information.
// Modifications copyright 2018 <Brad McDavid>

using System;
using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides a typed item collection
    /// </summary>
    public interface IItemCollection : IEnumerable<KeyValuePair<Type, object>>
    {
        /// <summary>
        /// Is a readonly only collection
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Get or set item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[Type key] { get; set; }
    }
}