using NCrontab;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maddalena.Core.Orleans
{
    class TaskGrain : Grain, IGrainWithStringKey, IStartupTask, ITaskGrain, IRemindable
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            foreach (var item in TaskService.Tasks)
            {
                await UpdateReminder(item);
            }
        }

        Task UpdateReminder(ScheduleTask task)
        {
            var schedule = CrontabSchedule.Parse(task.Period);
            var date = schedule.GetNextOccurrence(DateTime.Now);
            var dueTime = date - DateTime.Now;

            return RegisterOrUpdateReminder(task.Name, dueTime, TimeSpan.Zero);
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            var task = TaskService.Tasks.FirstOrDefault(x => x.Name == reminderName);
            await task.Body();
        }
    }
}