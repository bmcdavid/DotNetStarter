using System;
using DotNetStarter.Abstractions;
using System.Diagnostics;

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

    [Registration(typeof(RegistrationTestGreedyPrivate), Lifecycle.Transient)]
    public class RegistrationTestGreedyPrivate
    {
        private RegistrationTestGreedyPrivate(string a, string b, bool c, ITransient d)
        {

        }

        public RegistrationTestGreedyPrivate(ITransient d) { }
    }

    [Registration(typeof(RegistrationTestGreedyInternal), Lifecycle.Transient)]
    public class RegistrationTestGreedyInternal
    {
        private RegistrationTestGreedyInternal(string a, string b, bool c, ITransient d)
        {

        }

        public RegistrationTestGreedyInternal(ITransient d) { }
    }

    [Registration(typeof(RegistrationTestStatic), Lifecycle.Transient)]
    public class RegistrationTestStatic
    {
        static RegistrationTestStatic()
        {

        }

        public RegistrationTestStatic(ITransient d) { }
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

    [Registration(typeof(IFoo), Lifecycle.Singleton, typeof(FooServiceThree))]
    public class FooService : IFoo
    {
        public string Hello => "Hello, World!";
    }

    [Registration(typeof(IFoo), Lifecycle.Singleton, typeof(FooService))]
    public class FooServiceTwo : IFoo
    {
        public string Hello => "Hello, World part two!";
    }

    [Registration(typeof(IFoo), Lifecycle.Singleton)]
    public class FooServiceThree : IFoo
    {
        public string Hello => "Hello, World part three!";
    }

    [StartupModule]
    public class ShutdownMessage : IStartupModule
    {
        public void Shutdown()
        {
            Debug.WriteLine("DNS locator shutdown");
            Console.WriteLine("DNS locator shutdown");
        }

        public void Startup(IStartupEngine engine)
        {
            Debug.WriteLine("DNS Locator startup");
        }
    }
}