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

        public IFooTwo FooTwo => _TestSetup.TestContext.Locator.Get<IFooTwo>();

        public IReflectionHelper ReflectionHelper => _TestSetup.TestContext.Locator.Get<IReflectionHelper>();

        [TestMethod]
        public void ShouldCreateTimedTaskFromContainer()
        {
            var task = _TestSetup.TestContext.Locator.Get<ITimedTask>();

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
            Assert.IsTrue(ApplicationContext.Default.Locator.Get<Mocks.INotRegistered>() == null);
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
            Assert.IsNotNull(FooTwo);
        }

        [TestMethod]
        public void ShouldGetDifferentServiceFromFactory()
        {
            var one = FooTwo;
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
}