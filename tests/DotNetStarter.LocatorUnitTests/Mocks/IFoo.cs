using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests.Mocks
{
    public interface IFoo
    {
        string Hello { get; }
    }

    public interface IFooTwo
    {
        string SaySomething { get; }
    }

    [Registration(typeof(IFoo), Lifecycle.Singleton, typeof(FooServiceThree))]
    public class FooService : IFoo
    {
        public string Hello => "Hello, World!";
    }

    [Registration(typeof(IFoo), Lifecycle.Singleton)]
    public class FooServiceThree : IFoo
    {
        public string Hello => "Hello, World part three!";
    }

    [Registration(typeof(IFoo), Lifecycle.Singleton, typeof(FooService))]
    public class FooServiceTwo : IFoo
    {
        public string Hello => "Hello, World part two!";
    }
    public class FooTwo : IFooTwo
    {
        public string SaySomething => "yo";
    }

    public class FooTwoFactory
    {
        public static IFooTwo CreateFoo() => new FooTwo();
    }
}