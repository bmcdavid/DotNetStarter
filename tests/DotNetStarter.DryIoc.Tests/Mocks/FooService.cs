﻿using System;
using DotNetStarter.Abstractions;

namespace DotNetStarter.Tests.Mocks
{
    public interface IFoo
    {        
        string Hello { get; }
    }

    public interface ITransient
    {
        string Test();
    }

    [Register(typeof(ITransient), LifeTime.Transient)]
    public class TransientTest : ITransient
    {
        public string Test()
        {
            throw new NotImplementedException();
        }
    }

    [Register(typeof(IFoo), LifeTime.Singleton, typeof(FooServiceTwo))]
    public class FooService : IFoo
    {
        public string Hello => "Hello, World!";
    }

    [Register(typeof(IFoo), LifeTime.Singleton)]
    public class FooServiceTwo : IFoo
    {
        public string Hello => "Hello, World part two!";
    }

    [Register(typeof(IFoo), LifeTime.Singleton)]
    public class FooServiceThree : IFoo
    {
        public string Hello => "Hello, World part three!";
    }
}