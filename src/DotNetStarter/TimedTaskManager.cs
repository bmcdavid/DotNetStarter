namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Default timed task manager
    /// </summary>
    public class TimedTaskManager : ITimedTaskManager
    {
        private readonly List<ITimedTask> _applicationTasks = new List<ITimedTask>();

        private readonly IRequestSettingsProvider _requestSettingsProvider;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="requestSettingsProviderFactory"></param>
        public TimedTaskManager(Func<IRequestSettingsProvider> requestSettingsProviderFactory)
        {
            _requestSettingsProvider = requestSettingsProviderFactory() ?? throw new ArgumentNullException(nameof(requestSettingsProviderFactory));
        }

        /// <summary>
        /// Execute timed task
        /// </summary>
        /// <param name="task"></param>
        public virtual void Execute(ITimedTask task)
        {
            if (task is null) return;

            // if requiring debug, and not in debug mode, execute with no tracking
            if (task.RequireDebugMode && _requestSettingsProvider?.IsDebugMode != true)
            {
                task.TimedAction();
                return;
            }

            var s = Stopwatch.StartNew();
            task.TimedAction();
            s.Stop();
            task.Timer = s;
            task.Name = !string.IsNullOrWhiteSpace(task.Name) ?
                task.Name :
                "TimedTask:" + Guid.NewGuid();

            if (task.Scope == TimedActionScope.Application)
            {
                _applicationTasks.Add(task);
                return;
            }

            if (!ProviderHasItems(_requestSettingsProvider))
            {
                throw new Exception($"Settings provider or its Items are null and TimerScope is set to {nameof(TimedActionScope.Request)}!");
            }

            _requestSettingsProvider.Items[task.Name] = task;
        }

        /// <summary>
        /// Gets a timed task by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ITimedTask Get(string name)
        {
            var t = _applicationTasks.FirstOrDefault(x => Internal.CrossPlatformHelpers.StringCompareIgnoreCase(x.Name, name));

            if (t is object)
                return t;

            if (_requestSettingsProvider is null || !ProviderHasItems(_requestSettingsProvider))
                return null;

            return _requestSettingsProvider.Items[name] as TimedTask;
        }

        /// <summary>
        /// Gets all timed tasks matching prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public virtual IEnumerable<ITimedTask> GetAll(string prefix)
        {
            foreach (var t in GetApplicationTasks(prefix).Union(GetRequestTasks(prefix)))
            {
                yield return t;
            }
        }

        /// <summary>
        /// Determines if request settings provider has items and is implemented.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        protected virtual bool ProviderHasItems(IRequestSettingsProvider provider)
        {
            return provider?.Items is object;
        }

        private IEnumerable<ITimedTask> GetApplicationTasks(string prefix)
        {
            if (_applicationTasks is null)
            {
                yield break;
            }

            foreach (var task in _applicationTasks)
            {
                if (task.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    yield return task;
                }
            }
        }

        private IEnumerable<ITimedTask> GetRequestTasks(string prefix)
        {
            if (_requestSettingsProvider is null || !ProviderHasItems(_requestSettingsProvider))
            {
                yield break;
            }

            foreach (object key in _requestSettingsProvider.Items.Keys)
            {
                if (!(key is string name)) { continue; }
                if (_requestSettingsProvider.Items[name] is TimedTask task && task.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) == true)
                {
                    yield return task;
                }
            }
        }
    }
}