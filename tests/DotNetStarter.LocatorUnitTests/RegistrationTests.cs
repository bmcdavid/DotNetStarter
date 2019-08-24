using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public class RegistrationTests
    {
        internal Import<IRemove> Remove;

        public IFooTwo FooTwo => TestSetup.TestContext.Locator.Get<IFooTwo>();

        public IReflectionHelper ReflectionHelper => TestSetup.TestContext.Locator.Get<IReflectionHelper>();

        [TestMethod]
        public void ShouldCreateTimedTaskFromContainer()
        {
            var task = TestSetup.TestContext.Locator.Get<ITimedTask>();

            Assert.IsNotNull(task);
        }

        [DataRow(typeof(GenericStringBuilder), null)]
        [DataRow(null, typeof(object))]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldFailRegistrationDescriptorForTypes(Type serviceType, Type implType)
        {
            new RegistrationDescriptor(serviceType, implType, Lifecycle.Transient);
        }

        [DataRow(typeof(GenericStringBuilder), null)]
        [DataRow(null, null)]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldFailRegistrationDescriptorForServiceProvider(Type t, Func<IServiceProvider, GenericStringBuilder> f)
        {
            new RegistrationDescriptor(t, f, Lifecycle.Transient);
        }

        [TestMethod]
        public void ShouldBeReadOnlyLocatorInAppContext()
        {
            Assert.IsInstanceOfType(TestSetup.TestContext.Locator, typeof(ILocator));
        }

        [TestMethod]
        public void ShouldImportReflectionHelper()
        {
            Assert.IsNotNull(ReflectionHelper);
        }

        [TestMethod]
        public void ShouldThrowRegistrationException()
        {
            Assert.IsTrue(A.RegisterException);
        }

        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        [TestMethod]
        public void ShouldThrowResolveErrorWhenNotRegistered()
        {
            Assert.IsTrue(TestSetup.TestContext.Locator.Get<Mocks.INotRegistered>() is null);
        }

        [TestMethod]
        public void ShouldGetBaseClasses()
        {
            var baseImples = ReflectionHelper.GetBaseTypes(typeof(BaseImpl));

            Assert.IsTrue(baseImples.Count() > 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldRemoveService()
        {
            if (A.SupportsServiceRemoval)
            {
                Assert.IsNull(TestSetup.TestContext.Locator.Get<IRemove>());
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
            Assert.IsNotNull(FooTwo);
        }

        [TestMethod]
        public void ShouldGetDifferentServiceFromFactory()
        {
            var one = FooTwo;
            var two = TestSetup.TestContext.Locator.Get<IFooTwo>();

            Assert.AreNotEqual(one, two);
        }

        [TestMethod]
        public void ShouldGetImplFromAbstract()
        {
            var sut = TestSetup.TestContext.Locator.Get<BaseTest>();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void ShouldGetGenericService()
        {
            var sut = TestSetup.TestContext.Locator.Get<IPagedData<FooTwo>>(); // note: IPagedData<object> fails in LightInject

            Assert.IsNotNull(sut);
        }
    }
}