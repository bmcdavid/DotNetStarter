using DotNetStarter.Abstractions;

namespace DotNetStarter.Tests.Mocks
{
    public interface IFoo
    {        
        string Hello { get; }
    }

    [Register(typeof(IFoo), LifeTime.Container, ConstructorType.Greediest, typeof(FooServiceTwo))]
    public class FooService : IFoo
    {
        public string Hello => "Hello, World!";
    }

    [Register(typeof(IFoo), LifeTime.Transient)]
    public class FooServiceTwo : IFoo
    {
        public string Hello => "Hello, World part two!";
    }

    [Register(typeof(IFoo))]
    public class FooServiceThree : IFoo
    {
        public string Hello => "Hello, World part three!";
    }
}