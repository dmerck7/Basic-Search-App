using System;
using Scheduler.DataModels;
using Scheduler.NPoco;
using Scheduler.TSQL;
using NCrontab;
using NPoco;

namespace Scheduler.Utils
{
    class ScheduleUtils {
        // Calculate the next scheduled run for a task
        // Based off of a cron expression for the schedule
        public static DateTime CalculateNextRun(DateTime currentSched, string scheduleCron) {
            CrontabSchedule sched = CrontabSchedule.Parse(scheduleCron);

            DateTime today = DateTime.Today;
            DateTime end = new DateTime(2100, 12, 31);

            DateTime nextSched = sched.GetNextOccurrence(currentSched, end);

            bool isLastOfMonth = scheduleCron.Substring(0, scheduleCron.Length).Contains("28-31");
            if (isLastOfMonth) {
                while (nextSched.Day != DateTime.DaysInMonth(nextSched.Year, nextSched.Month)) {
                    nextSched = nextSched.AddDays(1);
                }
            }

            while (nextSched <= today) {
                nextSched = sched.GetNextOccurrence(nextSched, end);

                if (nextSched.Month == today.Month && isLastOfMonth) {
                    while (nextSched.Day != DateTime.DaysInMonth(nextSched.Year, nextSched.Month)) {
                        nextSched = nextSched.AddDays(1);
                    }
                }
            }

            return nextSched;
        }

        // Flip the bit field in the Scheduled Task Queue
        public static void UpdateBitField(Guid taskId, int bit) {
            using (IDatabase db = new NPocoDatabase("CommunicationCenter")) {
                var sql = Sql.Builder;
                sql.Append(ScheduledTaskTSQL.UpdateActiveBit(), new { @bitfield = bit, @taskId = taskId });
                db.Execute(sql);
            }
        }

        // Flip the bit field in the Scheduled Task Queue
        public static void UpdateTaskStatus(Guid taskId, Status status)
        {
            using (IDatabase db = new NPocoDatabase("CommunicationCenter"))
            {
                var sql = Sql.Builder;
                sql.Append(ScheduledTaskTSQL.UpdateTaskStatus(), new { @status = status, @taskId = taskId });
                db.Execute(sql);
            }
        }

        // Creates an entry in ScheduledTaskQueueHist for the task
        //public static void MoveToHistory(dynamic task, Guid histId, string message, DateTime startTime, DateTime endTime) {
        //    using (IDatabase db = new NPocoDatabase("CommunicationCenter")) {
        //        var sql = Sql.Builder;
        //        sql.Append(ScheduledTaskTSQL.SetToHistory(), new {
        //            @id = histId, @taskId = task.TaskId, @companyCode = task.CompanyCode, @notificationCompanyId = task.NotificationCompanyId,
        //            @scheduledTime = task.ScheduledTime, @taskType = task.TaskType, @status = message, @startTime = startTime,
        //            @endTime = endTime
        //        });
        //        db.Execute(sql);
        //    }
        //}

        // Update a task based on it's cron
        public static void UpdateScheduledTime(dynamic task, DateTime startTime, DateTime endTime) {
            using (IDatabase db = new NPocoDatabase("CommunicationCenter")) {
                var sql = Sql.Builder;
                sql.Append(ScheduledTaskTSQL.UpdateScheduledTask(),
                    new {
                        @scheduledTime = ScheduleUtils.CalculateNextRun(task.ScheduledTime, task.ScheduleCron),
                        @lastStartTime = startTime, @lastEndTime = endTime, @taskId = task.TaskId, @companyCode = task.CompanyCode
                    });
                db.Execute(sql);
            }
        }


    }
}
