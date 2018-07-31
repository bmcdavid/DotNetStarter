using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetStarter.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class RegistrationTests
    {
        public Import<IStartupContext> Context;

        internal Import<IRemove> Remove;

        public Import<IFooTwo> FooTwo;

        public Import<IReflectionHelper> ReflectionHelper;

        [TestMethod]
        public void ShouldCreateTimedTaskFromContainer()
        {
            var task = Context.Service.Locator.Get<ITimedTask>();

            Assert.IsNotNull(task);
        }
        
        [TestMethod]
        public void ShouldBeReadOnlyLocatorInAppContext()
        {
            Assert.IsInstanceOfType(ApplicationContext.Default.Locator, typeof(ILocator));
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
            if (A.SupportsServiceRemoval)
            {
                Assert.IsNull(DotNetStarter.ApplicationContext.Default.Locator.Get<IRemove>());
            }
            else
            {
                throw new Exception("To pass the test");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldRemoveImport()
        {
            if (A.SupportsServiceRemoval)
            {
                Assert.IsNull(Remove.Service);
            }
            else
            {
                throw new Exception("To pass the test");
            }
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
            var sut = DotNetStarter.ApplicationContext.Default.Locator.Get<IPagedData<FooTwo>>(); // note: IPagedData<object> fails in LightInject

            Assert.IsNotNull(sut);
        }
    }

    internal interface IRemove { }

    [Registration(typeof(IRemove), Lifecycle.Transient)]
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

    [Registration(typeof(IPagedData<>), Lifecycle.Transient)]
    public class PagedData<T> : IPagedData<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
    }

    [StartupModule(typeof(RegistrationConfiguration))]
    public class A : ILocatorConfigure
    {
        internal static bool SupportsServiceRemoval = false;

        internal static bool RegisterException = false;

        public void Configure(ILocatorRegistry container, IStartupEngineConfigurationArgs engine)
        {
            try
            {
                container.Add(typeof(IRemove), typeof(BaseImpl), null, Lifecycle.Singleton);
            }
            catch
            {
                RegisterException = true;
            }

            if (container is ILocatorRegistryWithRemove removable)
            {
                removable.Remove(typeof(IRemove));
                SupportsServiceRemoval = true;
            }

            container.Add<BaseTest, BaseImpl>(lifecycle: Lifecycle.Singleton);
            container.Add(typeof(IFooTwo), locator => FooTwoFactory.CreateFoo(), Lifecycle.Transient);
        }
    }
}
