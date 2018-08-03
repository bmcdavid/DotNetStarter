using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Locators
{
    internal class Configurations
    {
        public Type ConfigureType { get; set; }

        public ILocatorConfigure ConfigureInstance { get; set; }
    }
}