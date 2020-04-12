using DotNetStarter.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests.Mocks
{
    internal interface IInjectable { int Id { get; } }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(RegistrationTestGreedyInternal), Lifecycle.Transient)]
    public class RegistrationTestGreedyInternal
    {
        public RegistrationTestGreedyInternal(ITransient d) { }

        private RegistrationTestGreedyInternal(string a, string b, bool c, ITransient d) { }
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(RegistrationTestGreedyPrivate), Lifecycle.Transient)]
    public class RegistrationTestGreedyPrivate
    {
        public RegistrationTestGreedyPrivate(ITransient d) { }

        private RegistrationTestGreedyPrivate(string a, string b, bool c, ITransient d) { }
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(RegistrationTestStatic), Lifecycle.Transient)]
    public class RegistrationTestStatic
    {
        static RegistrationTestStatic() { }

        public RegistrationTestStatic(ITransient d) { }
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(ScopeTest), Lifecycle.Scoped)]
    internal class ScopeTest
    {
        public ScopeTest()
        {
            TestVariable = DateTime.Now.Ticks;
        }

        public long TestVariable { get; }
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(TestFuncCreationComplex), Lifecycle.Transient)]
    internal class TestFuncCreationComplex
    {
        public TestFuncCreationComplex(IInjectable injectionTest, IStartupConfiguration configuration, IShutdownHandler shutdownHandler)
        {
            InjectionTest = injectionTest;
        }

        public IInjectable InjectionTest { get; }
    }

    [ExcludeFromCodeCoverage]
    internal class TestInjectable : IInjectable
    {
        public int Id { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(TestLocatorInjectionScoped), Lifecycle.Scoped)]
    internal class TestLocatorInjectionScoped
    {
        public TestLocatorInjectionScoped(ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (null == locatorScopedAccessor.CurrentScope)
                throw new Exception("Scope not set!");
        }
    }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(TestLocatorInjectionTransient), Lifecycle.Scoped)]
    internal class TestLocatorInjectionTransient
    {
        public TestLocatorInjectionTransient(ILocatorScopedAccessor locatorScopedAccessor)
        {
            if (null == locatorScopedAccessor.CurrentScope)
                throw new Exception("Scope not set!");
        }
    }
}