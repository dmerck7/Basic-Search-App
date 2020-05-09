using System.Threading.Tasks.Dataflow;

using Scheduler.DataModels;

namespace Scheduler.ScheduledTasks
{
    class ScheduleAgent
    {

        public async void AgentInit(IReceivableSourceBlock<ScheduledTaskQueueDto> buffer)
        {
            while (await buffer.OutputAvailableAsync())
            {
                ScheduledTaskQueueDto task;
                while (buffer.TryReceive(out task))
                {
                    ProcessTask(task);
                }
            }
        }

        // Process all scheduled tasks (will expand as we schedule more things)
        private void ProcessTask(ScheduledTaskQueueDto task)
        {
            switch (task.TaskType)
            {
                case "Index":
                    TaskTypes.Index.ProcessIndex(task.TaskId);
                    break;
            }
        }

    }
}