using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using System;
using System.Collections.Generic;

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

    public class BaseImpl : BaseTest
    {
        public override string Prop => "Impl";
    }

    public abstract class BaseTest : IFooTwo
    {
        public abstract string Prop { get; }

        public string SaySomething => "Hi";
    }

    public class GenericObject : IGenericeMock<object> { }

    public class GenericStringBuilder : IGenericeMock<System.Text.StringBuilder> { }

    public abstract class MockBaseClass : IMock { }

    public class MockImplClass : MockBaseClass { }

    [Registration(typeof(IPagedData<>), Lifecycle.Transient)]
    public class PagedData<T> : IPagedData<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
    }

    internal class MockClass
    {
        public Import<object> Test { get; set; }
    }

    internal class MockFactory : ILocatorRegistryFactory
    {
        public ILocator CreateLocator() => throw new NotImplementedException();

        public ILocatorRegistry CreateRegistry() => throw new NotImplementedException();
    }

    internal class MockImportClass
    {
        public Import<NotRegisteredObject> Test { get; set; }

        public class NotRegisteredObject { }
    }

    internal class TestTimedTaskFactory
    {
        public static ITimedTask CreateTimedTask() => new TimedTask();
    }

    internal class RegistryFinalizer
    {
        public static bool Executed;

        public static void Finalize(ILocatorRegistry registry)
        {
            Executed = true;
        }
    }

    internal class TestContainerDefaults : Internal.ContainerDefaults { }

    internal class TestAssemblyScanner : AssemblyScanner { }

    internal class TestDependencyFinder : DependencyFinder { }

    internal class TestDependencySorter : DependencySorter
    {
        public static IDependencyNode CreateDependencyNode(object nodeType, Type attributeType)
        {
            return new DependencyNode(nodeType, attributeType);
        }

        public TestDependencySorter(Func<object, Type, IDependencyNode> n) : base(n) { }
    }

    internal class TestTimedTaskManager : TimedTaskManager
    {
        public TestTimedTaskManager(Func<IRequestSettingsProvider> p) : base(p)
        {

        }
    }
}