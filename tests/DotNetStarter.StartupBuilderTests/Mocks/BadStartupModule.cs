﻿using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.StartupBuilderTests.Mocks
{
    [StartupModule]
    public class BadStartupModule : IStartupModule
    {
        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Startup(IStartupEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}