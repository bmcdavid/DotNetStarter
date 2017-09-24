using System;
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

    /// <summary>
    /// Uses registration instead to ensure both types are working
    /// </summary>
    [Registration(typeof(ITransient), Lifecycle.Transient)]
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