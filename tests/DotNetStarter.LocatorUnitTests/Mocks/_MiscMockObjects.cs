using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using System;
using System.Collections.Generic;

[assembly: DiscoverTypes(typeof(IMock), typeof(IGenericeMock<>), typeof(IGenericeMock<object>), typeof(MockBaseClass))]

namespace DotNetStarter.UnitTests.Mocks
{
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
}