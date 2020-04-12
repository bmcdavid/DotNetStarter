using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[assembly: DiscoverTypes(typeof(IMock), typeof(IGenericeMock<>), typeof(IGenericeMock<object>), typeof(MockBaseClass))]

[assembly: Exports(ExportsType.All)]

namespace DotNetStarter.UnitTests.Mocks
{
    // todo: test a dynamic assembly, https://stackoverflow.com/questions/604501/generating-dll-assembly-dynamically-at-run-time

    public interface IGenericeMock<T> where T : new() { }

    public interface IMock { }

    public interface IPagedData<T>
    {
        IEnumerable<T> Items { get; set; }
        int TotalItems { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class BaseImpl : BaseTest
    {
        [ExcludeFromCodeCoverage]
        public override string Prop => "Impl";
    }

    [ExcludeFromCodeCoverage]
    public abstract class BaseTest : IFooTwo
    {
        [ExcludeFromCodeCoverage]
        public abstract string Prop { get; }

        public string SaySomething => "Hi";
    }

    [ExcludeFromCodeCoverage]
    public class GenericObject : IGenericeMock<object> { }

    [ExcludeFromCodeCoverage]
    public class GenericStringBuilder : IGenericeMock<System.Text.StringBuilder> { }

    [ExcludeFromCodeCoverage]
    public abstract class MockBaseClass : IMock { }

    [ExcludeFromCodeCoverage]
    public class MockImplClass : MockBaseClass { }

    [ExcludeFromCodeCoverage]
    [Registration(typeof(IPagedData<>), Lifecycle.Transient)]
    public class PagedData<T> : IPagedData<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class MockClass
    {
        public Import<object> Test { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class MockFactory : ILocatorRegistryFactory
    {
        public ILocator CreateLocator() => throw new NotImplementedException();

        public ILocatorRegistry CreateRegistry() => throw new NotImplementedException();
    }

    [ExcludeFromCodeCoverage]
    internal class MockImportClass
    {
        public Import<NotRegisteredObject> Test { get; set; }

        public class NotRegisteredObject { }
    }

    [ExcludeFromCodeCoverage]
    internal class TestTimedTaskFactory
    {
        public static ITimedTask CreateTimedTask() => new TimedTask();
    }

    [ExcludeFromCodeCoverage]
    internal class RegistryFinalizer
    {
        public static bool Executed;

        public static void Finalize(ILocatorRegistry registry)
        {
            Executed = true;
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestContainerDefaults : Internal.ContainerDefaults { }

    [ExcludeFromCodeCoverage]
    internal class TestAssemblyScanner : AssemblyScanner { }

    [ExcludeFromCodeCoverage]
    internal class TestDependencyFinder : DependencyFinder { }

    [ExcludeFromCodeCoverage]
    internal class TestDependencySorter : DependencySorter
    {
        public static IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return new DependencyNode(nodeType, attributeType);
        }

        public TestDependencySorter(Func<object, Type, IDependencyNode> n) : base(n) { }
    }

    [ExcludeFromCodeCoverage]
    internal class TestTimedTaskManager : TimedTaskManager
    {
        public TestTimedTaskManager(Func<IRequestSettingsProvider> p) : base(p)
        {

        }
    }
}