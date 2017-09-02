using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DotNetStarter.Tests
{
    [TestClass]
    public class RegistrationTests
    {
        Import<IStartupContext> Context;

        Import<IRemove> Remove;

        Import<IFooTwo> FooTwo;

        Import<IReflectionHelper> ReflectionHelper;

        [TestMethod]
        public void ShouldCreateTimedTaskFromContainer()
        {
            var task = Context.Service.Locator.Get<ITimedTask>();

            Assert.IsNotNull(task);
        }

        [TestMethod]
        public void ShouldImportReflectionHelper()
        {
            Assert.IsNotNull(ReflectionHelper.Service);
        }

        [TestMethod]
        public void ShouldThrowRegistrationException()
        {
            Assert.IsTrue(A.RegisterException);
        }

        [TestMethod]
        public void ShouldGetBaseClasses()
        {
            var baseImples = ReflectionHelper.Service.GetBaseTypes(typeof(BaseImpl));

            Assert.IsTrue(baseImples.Count() > 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldRemoveService()
        {
            Assert.IsNull(DotNetStarter.ApplicationContext.Default.Locator.Get<IRemove>());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldRemoveImport()
        {
            Assert.IsNull(Remove.Service);
        }

        [TestMethod]
        public void ShouldGetServiceFromFactory()
        {
            Assert.IsNotNull(FooTwo.Service);
        }

        [TestMethod]
        public void ShouldGetDifferentServiceFromFactory()
        {
            var one = FooTwo.Service;
            var two = DotNetStarter.ApplicationContext.Default.Locator.Get<IFooTwo>();

            Assert.AreNotEqual(one, two);
        }

        [TestMethod]
        public void ShouldGetImplFromAbstract()
        {
            var sut = DotNetStarter.ApplicationContext.Default.Locator.Get<BaseTest>();

            Assert.IsNotNull(sut);
        }


        [TestMethod]
        public void ShouldGetGenericService()
        {
            var sut = DotNetStarter.ApplicationContext.Default.Locator.Get<IPagedData<object>>();

            Assert.IsNotNull(sut);
        }
    }

    internal interface IRemove { }

    [Register(typeof(IRemove), LifeTime.Transient)]
    internal class Remove : IRemove { }

    public interface IFooTwo
    {
        string SaySomething();
    }

    public class FooTwo : IFooTwo
    {
        public string SaySomething() => "yo";
    }

    public class FooTwoFactory
    {
        public static IFooTwo CreateFoo() => new FooTwo();
    }

    public abstract class BaseTest : IFooTwo
    {
        public abstract string Prop { get; }

        public string SaySomething() => "Hi";
    }

    public class BaseImpl : BaseTest
    {
        public override string Prop => "Impl";
    }

    public interface IPagedData<T>
    {
        int TotalItems { get; set; }

        IEnumerable<T> Items { get; set; }
    }

    [Register(typeof(IPagedData<>), LifeTime.Transient)]
    public class PagedData<T> : IPagedData<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
    }

    [StartupModule(typeof(RegisterConfiguration))]
    public class A : ILocatorConfigure
    {
        internal static bool RegisterException = false;

        public void Configure(ILocatorRegistry container, IStartupEngine engine)
        {
            try
            {
                container.Add(typeof(IRemove), typeof(BaseImpl), null, LifeTime.Singleton);
            }
            catch
            {
                RegisterException = true;
            }

            container.Remove(typeof(IRemove));

            container.Add<BaseTest, BaseImpl>(lifetime: LifeTime.Singleton);

            container.Add(typeof(IFooTwo), locator => FooTwoFactory.CreateFoo(), LifeTime.Transient);
        }
    }
}
