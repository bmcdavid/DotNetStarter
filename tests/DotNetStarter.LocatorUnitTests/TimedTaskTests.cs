using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TimedTaskTests
    {
        [DataRow(true, DisplayName = "DebugMode")]
        [DataRow(false, DisplayName = "NotDebugMode")]
        [TestMethod]
        public void ShouldExecuteDebugTaskWhenDebugIsFalse(bool debugMode)
        {
            bool ran = false;
            var task = new TimedTask
            {
                RequireDebugMode = true,
                TimedAction = () => ran = true,
                Name = "TestTask",
                Scope = TimedActionScope.Request
            };

            var hash = new Hashtable();
            var provider = new TestRequestProvider(debugMode, hash);
            var sut = new TimedTaskManager(() => provider);
            sut.Execute(task);

            Assert.IsTrue(ran);

            if (debugMode)
            {
                Assert.IsNotNull(sut.Get(task.Name));
            }
            else
            {
                Assert.IsNull(sut.Get(task.Name));
            }

        }

        [TestMethod]
        public void ShouldGetAllRequestTasksPrefixed()
        {
            var dictionary = new Hashtable
            {
                { new object(), new object() }
            };
            var provider = new TestRequestProvider(true, dictionary);
            var manager = new TimedTaskManager(() => provider);
            var task = new TimedTask { Name = "test", TimedAction = () => { }, Scope = TimedActionScope.Request };
            var task2 = new TimedTask { Name = "test2", TimedAction = () => { }, Scope = TimedActionScope.Application };
            var task3 = new TimedTask { Name = "anotherTask", TimedAction = () => { }, Scope = TimedActionScope.Application };
            var task4 = new TimedTask { Name = "anotherTask", TimedAction = () => { }, Scope = TimedActionScope.Request };

            manager.Execute(task);
            manager.Execute(task2);
            manager.Execute(task3);
            manager.Execute(task4);

            Assert.IsTrue(manager.GetAll("test").Count() == 2);
            Assert.IsNotNull(manager.Get("anotherTask"));
        }

        [TestMethod]
        public void ShouldHaveTimers()
        {
            var timers = TestSetup.TestContext.Configuration.TimedTaskManager.GetAll(nameof(DotNetStarter));

            Assert.IsTrue(timers.Any());
        }

        [TestMethod]
        public void ShouldNotBeDebugForRequestProvider()
        {
            Assert.IsFalse(new RequestSettingsProvider().IsDebugMode);
        }

        [TestMethod]
        public void ShouldNotExecuteNullTaskOrThrowException()
        {
            new TimedTaskManager(() => new TestRequestProvider(false, null)).Execute(null);
        }

        [TestMethod]
        public void ShouldNotHaveAnyRequetTasksWhenProviderEmptyItems()
        {
            var provider = new TestRequestProvider(false, new Hashtable());
            var sut = new TimedTaskManager(() => provider);

            Assert.IsFalse(sut.GetAll("test").Any());
            Assert.IsNull(sut.Get("test"));
        }

        [TestMethod]
        public void ShouldNotHaveAnyRequetTasksWhenProviderNullItems()
        {
            var provider = new TestRequestProvider(false, null);
            var sut = new TimedTaskManager(() => provider);

            Assert.IsFalse(sut.GetAll("test").Any());
            Assert.IsNull(sut.Get("test"));
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldThrowArgumentNullExceptionOnNullFuncProvider()
        {
            new TimedTaskManager(() => null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldThrowArgumentNullExceptionOnNullProvider()
        {
            new TimedTaskManager(null);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ShouldThrowExceptionWhenNullRequestSettingsProviderWithScopeTask()
        {
            var sut = new TimedTaskManager(() => new TestRequestProvider(false, null));
            sut.Execute(new TimedTask { TimedAction = () => { }, Scope = TimedActionScope.Request });
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void ShouldThrowExceptionWithDefaultRequestProvider()
        {
           var _ = new RequestSettingsProvider().Items["a"];
        }
        private class TestRequestProvider : IRequestSettingsProvider
        {
            public TestRequestProvider(bool isDebug, IDictionary dictionary)
            {
                IsDebugMode = isDebug;
                Items = dictionary;
            }

            public bool IsDebugMode { get; }
            public IDictionary Items { get; }
        }
    }
}