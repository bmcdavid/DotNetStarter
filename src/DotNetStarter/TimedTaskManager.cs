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
        private IRequestSettingsProvider RequestSettingsProvider;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="requestSettingsProviderFactory"></param>
        public TimedTaskManager(Func<IRequestSettingsProvider> requestSettingsProviderFactory)
        {
            RequestSettingsProvider = requestSettingsProviderFactory() ?? throw new ArgumentNullException(nameof(requestSettingsProviderFactory));
        }

        /// <summary>
        /// Store all timed tasks
        /// </summary>
        private List<ITimedTask> _ApplicationTasks = new List<ITimedTask>();

        /// <summary>
        /// Execute timed task
        /// </summary>
        /// <param name="task"></param>
        public virtual void Execute(ITimedTask task)
        {
            if (task == null) return;

            // if requiring debug, and not in debug mode, execute with no tracking
            if (task.RequireDebugMode && RequestSettingsProvider?.IsDebugMode != true)
            {
                task.TimedAction();

                return;
            }

            Stopwatch s = new Stopwatch();
            s.Start();
            task.TimedAction();
            s.Stop();

            if (string.IsNullOrEmpty(task.Name))
                task.Name = "TimedTask:" + Guid.NewGuid();

            task.Timer = s;

            if (task.Scope == TimedActionScope.Application)
                _ApplicationTasks.Add(task);
            else
            {
                if (!ProviderHasItems(RequestSettingsProvider))
                    throw new Exception($"Settings provider or its Items are null and TimerScope is set to {nameof(TimedActionScope.Request)}!");

                RequestSettingsProvider.Items.Remove(task.Name);
                RequestSettingsProvider.Items.Add(task.Name, task);
            }
        }

        /// <summary>
        /// Gets a timed task by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ITimedTask Get(string name)
        {
            ITimedTask t = _ApplicationTasks.FirstOrDefault(x => Internal.CrossPlatformHelpers.StringCompareIgnoreCase(x.Name, name));

            if (t != null)
                return t;

            if (RequestSettingsProvider == null || !ProviderHasItems(RequestSettingsProvider))
                return null;

            return RequestSettingsProvider.Items[name] as TimedTask;
        }

        /// <summary>
        /// Gets all timed tasks matching prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public virtual IEnumerable<ITimedTask> GetAll(string prefix)
        {
            if (_ApplicationTasks == null)
                yield break;

            // Get all in application
            foreach (var task in _ApplicationTasks)
            {
                if (task.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    yield return task;
            }

            //Get all in request
            if (RequestSettingsProvider == null || !ProviderHasItems(RequestSettingsProvider))
                yield break;

            foreach (object key in RequestSettingsProvider.Items.Keys)
            {
                string name = key as string;

                if (name == null) continue;

                var task = RequestSettingsProvider.Items[name] as TimedTask;

                if (task?.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) == true)
                    yield return task;
            }
        }

        /// <summary>
        /// Determines if request settings provider has items and is implemented.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        protected virtual bool ProviderHasItems(IRequestSettingsProvider provider)
        {
            try
            {
                return provider?.Items != null;
            }
            catch (NotImplementedException)
            {
                return false;
            }
        }
    }
}