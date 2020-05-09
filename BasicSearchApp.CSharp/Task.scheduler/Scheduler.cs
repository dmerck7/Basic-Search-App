using NPoco;
using System;
using System.Threading;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

using Scheduler.TSQL;
using Scheduler.NPoco;
using Scheduler.Utils;
using Scheduler.DataModels;
using Scheduler.ScheduledTasks;

namespace Scheduler {
    // Turns the Scheduler into a Windows Service
    public class Scheduler : ServiceBase {
        public Scheduler() {

            using (IDatabase db = new NPocoDatabase("CommunicationCenter"))
            {
                var sql = Sql.Builder;
                sql.Append(ScheduledTaskTSQL.ResetAllProcessingTasks());
                db.Execute(sql);
            }

            Thread thread = new Thread(CreateAgents);
            thread.Start();
        }

        // Create the agents to process Notifications
        private static void CreateAgents() {
            BufferBlock<ScheduledTaskQueueDto> buffer = new BufferBlock<ScheduledTaskQueueDto>();

            ScheduleAgent agent1 = new ScheduleAgent();
            ScheduleAgent agent2 = new ScheduleAgent();
            ScheduleAgent agent3 = new ScheduleAgent();

            agent1.AgentInit(buffer);
            agent2.AgentInit(buffer);
            agent3.AgentInit(buffer);

            RunScheduledTasks(buffer);
        }

        // Pull the scheduled tasks from the Queue
        private static void RunScheduledTasks(BufferBlock<ScheduledTaskQueueDto> buffer) {
            while (true) {
                List<ScheduledTaskQueueDto> tasks = new List<ScheduledTaskQueueDto>();

                using (IDatabase db = new NPocoDatabase("CommunicationCenter")) {
                    var sql = Sql.Builder;
                    sql.Append(ScheduledTaskTSQL.GetReadyTasks());
                    tasks = db.Fetch<ScheduledTaskQueueDto>(sql);
                }

                foreach(ScheduledTaskQueueDto task in tasks) {
                    buffer.Post(task);
                    ScheduleUtils.UpdateBitField(task.TaskId, 0);
                }

                Thread.Sleep(new TimeSpan(0, 1, 0));    // 1 minute interval
            }
        }
    }
}
