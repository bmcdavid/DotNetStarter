using DotNetStarter.Abstractions;
using System.Diagnostics.CodeAnalysis;

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

    [ExcludeFromCodeCoverage]
    [Registration(typeof(IFoo), Lifecycle.Singleton, typeof(FooServiceThree))]
    public class FooService : IFoo
    {
        public string Hello => "Hello, World!";
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(IFoo), Lifecycle.Singleton)]
    public class FooServiceThree : IFoo
    {
        public string Hello => "Hello, World part three!";
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(IFoo), Lifecycle.Singleton, typeof(FooService))]
    public class FooServiceTwo : IFoo
    {
        public string Hello => "Hello, World part two!";
    }

    [ExcludeFromCodeCoverage]
    public class FooTwo : IFooTwo
    {
        public string SaySomething => "yo";
    }

    [ExcludeFromCodeCoverage]
    public class FooTwoFactory
    {
        public static IFooTwo CreateFoo() => new FooTwo();
    }

    [ExcludeFromCodeCoverage]
    public class FooTransient { }

    [ExcludeFromCodeCoverage]
    public class FooScoped { }

    [ExcludeFromCodeCoverage]
    public class FooSingleton { }
}